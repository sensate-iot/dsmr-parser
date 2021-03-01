# Sensate IoT - Smart Energy

The Sensate IoT Smart Energy project implements an IoT solution for (Dutch)
Smart Meters. The project consists of several repository's:

	- DSMR parser (this repo);
	- DSMR web client (implementator of this service)
	- Several customer facing apps

## DSMR parser

The DSMR parser receives requests from the web client with a DSMR telegram. Telegrams
are data messages from Dutch smart meters. These messages are formatted according
to the DSMR standard. This parser supports the following versions of DSMR:

	- DSMR 2.2
	- DSMR 4.x
	- ESMR 5.x

## WCF

This service exposes a WCF `Parse` endpoint. Clients can implement the contract in oder to
communicate with this service and parse messages. The contract definition can be found in
the `SensateIoT.SmartEnergy.Dsmr.Parser` project.

## Enhancements

This services enhances the DSMR telegram in serveral ways. First of all, using a small cache
the gas flow is calculated using previously received messages. If no previous messages are found
the gas flow is defaulted to 0.
