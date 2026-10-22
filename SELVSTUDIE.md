# Dag 5 · Beskeder mellem to programmer

Brug Dag05-TCP-og-UDP.pptx med 21 slides. Dagen er selvstudie på cirka 2–3 timer inklusive læsning, afprøvning og pauser. Ingen oplæg, obligatorisk video eller aflevering.

## Formål

I undersøger, hvordan to programmer udveksler beskeder, og hvilke regler programmerne selv skal aftale. Efter dagen skal I kunne forklare, hvorfor vores TCP-program bruger linjeskift, og hvorfor afsendelse med UDP ikke beviser modtagelse. I behøver ikke kunne skrive en socket-server fra bunden.

## Rækkefølge

- Slide 1–4: læs formålet og planen, åbn NetvaerkLabs.slnx, og find de tre TODO-steder.
- Slide 5–14: udfyld TCP-client og server. Test PING, ECHO, genstart af clienten og en stoppet server.
- Slide 15–18: udfyld UDP-senderen. Test med og uden modtageren og send igen efter genstart.
- Slide 19–21: sammenlign forsøgene og kontrollér, at I kan forklare resultaterne.

## Byg og start i Visual Studio

Vælg Build → Build Solution. Højreklik på projektet i Solution Explorer, og vælg Debug → Start Without Debugging. Start TcpServer før TcpClient og UdpReceiver før UdpSender. Lad serverens eller modtagerens vindue være åbent. Start kun én server eller modtager pr. port.

Hvis menupunktet mangler, vælg Set as Startup Project og derefter Debug → Start Without Debugging (Ctrl+F5). Skift startprojekt til det næste program, og gentag, mens serveren fortsat kører.

Stop servere og modtagere med Ctrl+C i deres egne konsolvinduer før rettelser og nyt build. En tom linje afslutter TcpClient; UdpSender afslutter selv. Programmer startet uden debugger kan fortsat køre, selv om Visual Studio ikke viser en aktiv debugging-session.

## Støtte og afslutning

HJAELP.md indeholder løsninger og fejlfinding. Skabeloner indeholder komplette støttefiler. Terminalkommandoerne i README.md er et valgfrit alternativ.

I bruger kun TcpServer, TcpClient, UdpReceiver og UdpSender. De øvrige projekter og Bogkatalog.slnx skal ikke bruges fredag. Når de to forsøg er gennemført og forklaret, kan I afslutte dagen.

Unicast, broadcast og multicast behandles sammen på dag 6; client-server og P2P på dag 7. FORDYBELSE.md er separat, frivilligt materiale til senere brug og er ikke en del af fredagens arbejde.
