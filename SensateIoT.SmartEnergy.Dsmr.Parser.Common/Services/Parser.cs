using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Attributes;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Services
{
    public class Parser : IParser
    {
	    private delegate void TelegramParsedEventHandler(object sender, Telegram telegram);

        private const char telegramStart = '/';
        private const char telegramEnd = '!';
        private const char valueStart = '(';
        private const char valueEnd = ')';
        
        public async Task<Telegram> Parse(string message)
        {
            Telegram result = null;

            using (var reader = new StringReader(message))
            {
                await this.ParseFromStringReader(reader, (sender, telegram) => {
                    if(result == null) {
	                    result = telegram;
                    }
                });
            }

            return result;
        }
        
        private async Task ParseFromStringReader(StringReader reader, TelegramParsedEventHandler onParsedEvent)
        {
	        string line;
            Telegram telegram = null;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (telegram == null)
                {
                    if (line.StartsWith(telegramStart.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        telegram = new Telegram();
                        SetTelegramHeader(ref telegram, line);
                    }
                }
                else
                {
                    if (line.StartsWith(telegramEnd.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        SetTelegramCRC(ref telegram, line);
                        onParsedEvent?.Invoke(this, telegram);
                        telegram = null;
                    }
                    else
                    {
                        SetTelegramContent(ref telegram, line);
                    }
                }
            }
        }

        private static void SetTelegramContent(ref Telegram telegram, string line)
        {
            var parsed = ParseContentLine(line).ToList();
            var properties = GetPropertiesWithKey(parsed.ElementAtOrDefault(0)).ToList();

            if (parsed.Any() && parsed.Count() > 1 && properties.Any())
            {
                var values = parsed.Skip(1);
                SetTelegramProperties(ref telegram, properties, values.ToList());
            }

            telegram.Lines.Add(line);
        }

        private static IEnumerable<string> ParseContentLine(string line)
        {
            var parts = line.Split(valueStart);
            for (var i = 0; i < parts.Length; i++)
            {
                var endIndex = parts[i].IndexOf(valueEnd);
                if (endIndex > -1)
                {
                    parts[i] = parts[i].Substring(0, endIndex);
                }
                yield return parts[i];
            }
        }

        private static IEnumerable<string> GetPropertiesWithKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                yield return null;
            }

            foreach (var property in GetTelegramProperties())
            {
                var attr = property.GetCustomAttributes(typeof(ObisAttribute), false).Cast<ObisAttribute>().FirstOrDefault();
                if (attr != null && attr.ObisIdentifier == key)
                {
                    yield return property.Name;
                }
            }
        }

        private static void SetTelegramHeader(ref Telegram telegram, string value)
        {
            var header = value.Replace(telegramStart.ToString(), string.Empty);
            var propertyInfo = GetTelegramProperty(nameof(Telegram.MessageHeader));
            SetPropertyValue(ref telegram, propertyInfo, header);
            telegram.Lines.Add(value);
        }

        private static void SetTelegramCRC(ref Telegram telegram, string value)
        {
            if (telegram.MessageVersion == ObisVersion.V42 || telegram.MessageVersion == ObisVersion.V50) {
                var crc = value.Replace(telegramEnd.ToString(), string.Empty);
                crc = crc.Length > 4 ? crc.Substring(0, 4) : crc;
                var propertyInfo = GetTelegramProperty(nameof(Telegram.CRC));

                SetPropertyValue(ref telegram, propertyInfo, crc);
                telegram.Lines.Add(telegramEnd + crc);
            } else {
                telegram.Lines.Add(telegramEnd.ToString());
            }
        }

        private static void SetTelegramProperties(ref Telegram telegram, ICollection<string> properties, ICollection<string> values)
        {
            var telegramProperties = GetTelegramProperties().Where(p => properties.Contains(p.Name));

            foreach (var propertyInfo in telegramProperties)
            {
                var obisAttribute =
                    propertyInfo.GetCustomAttributes(typeof(ObisAttribute), false)
                        .Cast<ObisAttribute>()
                        .FirstOrDefault();

                if (obisAttribute == null) {
                    continue;
                }

                var valueForProperty = values.ElementAtOrDefault(obisAttribute.ValueIndex);

                if (valueForProperty == null) {
                    continue;
                }

                SetPropertyValue(ref telegram, propertyInfo, valueForProperty, obisAttribute.ValueUnit);
            }
        }

        private static void SetPropertyValue(ref Telegram telegram, PropertyInfo propertyInfo, string value, string obisValueUnit = null)
        {
            var convertedValue = GetConvertedPropertyValue(propertyInfo, value, obisValueUnit);
            propertyInfo.SetValue(telegram, convertedValue);
        }

        private static object GetConvertedPropertyValue(PropertyInfo propertyInfo, string value, string obisValueUnit = null)
        {
            var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
            var converterAttribute =
                propertyInfo.GetCustomAttributes(typeof(TypeConverterAttribute), false)
                    .Cast<TypeConverterAttribute>()
                    .FirstOrDefault();

            if (converterAttribute != null) {
                var converterType = Type.GetType(converterAttribute.ConverterTypeName);

                if(converterType == null) {
	                throw new InvalidOperationException($"Unable to determine converter type for: {converterAttribute.ConverterTypeName}");
                }

                converter = Activator.CreateInstance(converterType) as TypeConverter;
            }

            if (!string.IsNullOrEmpty(obisValueUnit)) {
                value = value.Replace("*" + obisValueUnit, string.Empty);
            }

            return converter?.ConvertFromInvariantString(value);
        }

        private static IEnumerable<PropertyInfo> GetTelegramProperties()
        {
            return typeof(Telegram).GetTypeInfo().DeclaredProperties;
        }

        private static PropertyInfo GetTelegramProperty(string propertyName)
        {
            return typeof(Telegram).GetTypeInfo().GetDeclaredProperty(propertyName);
        }
    }
}