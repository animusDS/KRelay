# KRelay
## KRelay 1.6.0 | RotMG 1.6.0.0.0
### A modular Realm of the Mad God man-in-the-middle Proxy

![Screenshot](/Screenshot.png)
-----------------------------------------------------------

## Setting Up (Assuming you have a compiled binary)
1. If the FiddlerScript is already set up/you have your own way of HTTP MITM proxying in/injecting in the Proxy Server, jump to 5.
2. Download and install [Fiddler](https://www.telerik.com/download/fiddler/fiddler4)
3. Head over to the FiddlerScript tab
4. Insert the following into the OnBeforeResponse function
```XML
oSession.utilDecodeResponse();
oSession.utilReplaceInResponse('<Servers>',
    '<Servers>' +
        '<Server>' +
            '<Name>USSouthWest2</Name>' +
            '<DNS>127.0.0.1</DNS>' +
            '<Lat>32.80</Lat>' +
            '<Long>-96.77</Long>' +
            '<Usage>0.00</Usage>' +
        '</Server>');
```
5. Open Fiddler, K Relay and Exalt
6. Connect to USSW2 (the proxy server)

## K Relay Support Discord Server
Join for help with K Relay, plugin discussions, etc.: https://discord.gg/AUyH8aUj2C

## Plugin Documentation
[Can be found here](https://github.com/TheKronks/K_Relay_Plugin_Documentation/blob/master/README.md)
