# Dag 5 · TCP og UDP i C#

Fredagens eneste fokus er tekstbeskeder med TCP og datagrammer med UDP. Adresseringsformer behandles på dag 6 og P2P på dag 7. De øvrige console-projekter er ikke fredagens arbejde. Dagen er selvstudie uden oplæg eller krav om video. Begynd med [selvstudievejledningen](SELVSTUDIE.md), som viser rækkefølgen, kerneøvelserne og kontrolpunkterne.

Åbn NetvaerkLabs.slnx til dagens øvelser. Bogkatalog.slnx viderefører Web/API-løsningen fra dag 4 som reference. Bogkatalog har API på 5200 og Web på 5100. Dagens console-programmer erstatter ikke HTTP-API'et.

## Byg og start i Visual Studio

Åbn NetvaerkLabs.slnx. Vælg Build → Build Solution. Højreklik derefter på det ønskede projekt i Solution Explorer, og vælg Debug → Start Without Debugging.

Start TcpServer først og derefter TcpClient. Lad serverens konsolvindue være åbent, mens clienten startes. Ved UDP startes UdpReceiver først og derefter UdpSender. Hvert program får sit eget konsolvindue. Start kun én server eller modtager pr. port.

Hvis genvejen ikke findes i projektets menu, vælger I Set as Startup Project og derefter Debug → Start Without Debugging (Ctrl+F5). Skift derefter startprojekt til det næste program, og gentag; lad den allerede startede server eller modtager køre.

Stop servere og modtagere med Ctrl+C i deres egne konsolvinduer før rettelser og nyt build. En tom linje afslutter TcpClient. UdpSender afslutter selv efter afsendelsen. Hvis konsollen viser, at programmet er afsluttet, kan vinduet lukkes. Visual Studios Stop Debugging afslutter ikke nødvendigvis programmer startet uden debugger.

## Valgfrit alternativ · terminaler
Åbn en PowerShell-terminal i denne mappe for hvert program. Stop servere og modtagere med Ctrl+C før genstart eller build. Kør kun én kopi pr. normal listener-port.

```
dotnet build NetvaerkLabs.slnx
dotnet run --project NetvaerkLabs/TcpServer -- 6000
dotnet run --project NetvaerkLabs/TcpClient -- 127.0.0.1 6000
dotnet run --project NetvaerkLabs/UdpReceiver -- 6100
dotnet run --project NetvaerkLabs/UdpSender -- 127.0.0.1 6100
```

Standardværdierne ved start fra Visual Studio svarer til kommandoerne ovenfor.

## Fredagens arbejde i startprojektet
TcpClient.ExchangeAsync har TODO til Grøn A. TcpServer.HandleClientAsync har TODO til Grøn B. UdpSender.SendMessageAsync har TODO til Grøn C. Modtageren er færdig. Startprojektet bygger allerede, men beskedudvekslingen virker først efter øvelserne. Skabeloner indeholder de tre komplette programfiler som støtte til opgaverne. Øvelsestrin og forventede resultater står i PowerPoint.

TCP-kontrakten er UTF-8 uden BOM og én besked pr. linje. PING giver PONG, og anden tekst giver ECHO efterfulgt af teksten. Tom linje i clientens konsol afslutter clienten. Clienten lukker ved timeout efter fem sekunder. Eksemplet er til betroede øvelsesbeskeder: det har ikke produktionsgrænser for linjelængde eller antal clients.

Fredagens TCP-server håndterer én forbindelse ad gangen. I frivillig fordybelse til senere erstattes dette med flere samtidige forbindelser. Ændringen erstatter await HandleClientAsync(client) med _ = HandleClientAsync(client). Metoden ejer forbindelsen og håndterer netværksfejl. Det er en lille undervisningsserver uden fælles lager eller krav om en tråd pr. client.

UDP-senderen sender fem nummererede datagrammer. SendAsync bekræfter ikke levering. UdpReceiver kan startes med --drop-every-3 for at simulere, at programmet ignorerer hver tredje modtagne besked. Det er kontrolleret applikationsadfærd, ikke observeret pakketab i netværket.

## Frivillig fordybelse til senere · discovery
```
dotnet run --project NetvaerkLabs/DiscoveryServer -- 6200
dotnet run --project NetvaerkLabs/DiscoveryClient -- 127.0.0.1 6200
dotnet run --project NetvaerkLabs/DiscoveryClient
```
Første clientkommando er unicast og virker som lokal kontrol. Uden argument bruger clienten broadcast 255.255.255.255. Serveren svarer med BOGSERVER tcp=6000 til forespørgslens afsender. Clienten samler svar i tre sekunder. Start TcpServer separat, og brug svarafsenderens IP ved et efterfølgende TCP-kald. Discovery starter ikke TCP-tjenesten og vælger ikke automatisk en server. UDP 6200 er discovery, TCP 6000 er tjenesten. Et discovery-svar er ikke autentificeret.

## Frivillig fordybelse til senere · LAN og multicast
Brug kun et aftalt testnet. TcpServer og UdpReceiver har flaget --lan til at lytte på alle lokale IPv4-adresser. Clients bruger den faktiske IPv4-adresse på det aktive interface. Slå ikke firewall fra. Hvis netværket blokerer broadcast eller multicast, fortsætter I med unicast på localhost.

```
dotnet run --project NetvaerkLabs/TcpServer -- 6000 --lan
dotnet run --project NetvaerkLabs/UdpReceiver -- 6100 --lan
dotnet run --project NetvaerkLabs/MulticastReceiver -- 127.0.0.1 6300
dotnet run --project NetvaerkLabs/MulticastSender -- 127.0.0.1 6300
```
Den automatiske kontrol modtog ikke multicast på loopback i dette Windows-miljø. Derfor er loopback-kommandoerne et forsøg, ikke et garanteret minimumsresultat. Brug kun et aftalt fysisk testnet til yderligere kontrol.

Multicast bruger 239.255.42.99, port 6300 og TTL 1. Start modtager før sender. På et fysisk LAN erstatter I 127.0.0.1 med den pågældende pc's aktive IPv4-interface i begge programmer. Portdeling og lokal multicast afhænger af operativsystem og netværk. Receiver-programmet forbereder portdeling, så to lokale modtagere kan forsøges. Det er ikke en leveringsgaranti.

## Til GitHub
Udgiv kun denne startprojektmappe, inklusive NetvaerkLabs og Skabeloner. Mapperne bin, obj og .vs hører ikke med. Løsningsforslaget holdes separat. Der er ikke behov for at udlevere en ekstra ZIP, når repoet er tilgængeligt.
