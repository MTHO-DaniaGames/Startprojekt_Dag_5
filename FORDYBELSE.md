# Frivillig fordybelse · dag 5

Dette er separat, frivilligt materiale til senere brug. Det er ikke en del af fredagens arbejde. Begreberne adresseringsformer og P2P behandles sammen på dag 6 og 7, før eventuel selvstændig afprøvning. Der er ingen aflevering.

## Gul · Afprøv server med én client ad gangen

Start TcpServer og derefter client A. Lad A stå åben uden at sende mere.
Start client B, og send PING. B kan få en TCP-forbindelse, men den sekventielle server har endnu ikke startet sin behandling af B. Clienten kan derfor nå sin timeout.
Afslut A med en tom linje. Start derefter en frisk B og send PING igen. Forvent PONG.


## Asynkron behandling af flere forbindelser

I hovedløkken ændres await til at starte handleren uden at vente på dens afslutning. Listeneren kan derefter acceptere næste client.
HandleClientAsync ejer stadig sin forbindelse og håndterer netværksfejl. Hver handler har sin egen reader og writer.

```csharp
// Før: én client ad gangen
await HandleClientAsync(client);

// Efter: næste client kan accepteres
_ = HandleClientAsync(client);
```


## Samtidighed og delt tilstand

Async gør det muligt at vente på netværk uden at blokere hovedløkken. Det kræver ikke en permanent tråd pr. client.
Vores handlers deler ikke en bogliste. Hvis flere clients ændrer et fælles lager, skal samtidighed og korrekt opdatering løses særskilt.
Undervisningseksemplet begrænser ikke antallet af forbindelser. En produktionsserver skal styre belastning, fejl og nedlukning mere systematisk.


## Gul · Lad to clients få svar

Stop serveren. Ret linjen i TcpServers hovedløkke til _ = HandleClientAsync(client), og byg igen.
Start serveren og to clients. Lad A blive åben, mens B sender PING. Nu skal B få PONG uden at vente på, at A afslutter.
Send også tekst fra A. Begge forbindelser skal fortsat kunne udveksle deres egne beskeder.


## Kontrolleret ignorering i programmet

Flaget --drop-every-3 er forberedt i UdpReceiver. Programmet modtager stadig datagrammet, men springer behandlingen over.
Konsollen markerer det som SIMULERET. Det er en illustration af manglende behandling, ikke et bevis på pakketab i netværket.

```csharp
dotnet run --project NetvaerkLabs/UdpReceiver -- 6100 --drop-every-3
```


## Gul · Se et hul i beskedrækken

Stop den almindelige UdpReceiver. Start den med --drop-every-3 med kommandoen i afsnittet ovenfor, og kør UdpSender.
Forvent besked 1 og 2, en markering med SIMULERET og derefter besked 4 og 5.
Genstart modtageren uden flaget, og send igen. Bemærk, at sekvensnumre kan afsløre manglende behandling, men ikke reparere den.


## Serveren svarer på en kendt forespørgsel

DiscoveryServer er forberedt. Den svarer kun på vores aftalte søgetekst og sender svaret til datagrammets afsender.
Svaret annoncerer en tjeneste. Det kontrollerer ikke, om TcpServer allerede kører.

```csharp
if (Encoding.UTF8.GetString(packet.Buffer) != "FIND_BOGSERVER")
    continue;
byte[] reply = Encoding.UTF8.GetBytes("BOGSERVER tcp=6000");
await receiver.SendAsync(reply, packet.RemoteEndPoint);
```


## Clientens broadcast og svarfrist

EnableBroadcast tillader UDP-broadcast. Clienten får en midlertidig lokal port til svarene.
Clienten indsamler svar i tre sekunder. Hele programmet og fejlhåndteringen ligger i DiscoveryClient.

```csharp
client.EnableBroadcast = true;
await client.SendAsync(query,
    new IPEndPoint(IPAddress.Broadcast, 6200));

using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(3));
UdpReceiveResult reply = await client.ReceiveAsync(timeout.Token);
```


## Lokal kontrol før broadcast

Start DiscoveryServer i én terminal. Kør clienten med 127.0.0.1 i en anden terminal. Det er en unicast-kontrol af samme beskedkontrakt.
Forvent Fundet 127.0.0.1:6000. Denne test beviser ikke, at broadcast på skolens netværk virker.

```csharp
dotnet run --project NetvaerkLabs/DiscoveryServer

dotnet run --project NetvaerkLabs/DiscoveryClient -- 127.0.0.1
```


## Gul · Afprøv serveropdagelse

Kør først den lokale unicast-kontrol. Start også TcpServer på 6000, og kontrollér PING/PONG med TcpClient.
På et aftalt testnet: kør DiscoveryClient uden argument. Nu bruges broadcast. Forvent et svar, hvis netværket og firewall tillader det.
Hvis søgningen ikke får svar, fortsætter I med unicast-kontrollen. Slå ikke firewall fra, og ændr ikke skolens netværksregler.


## Gul · Brug den fundne serveradresse

Hvis to pc’er kan nå hinanden, starter Pc B TcpServer med argumenterne 6000 --lan og starter DiscoveryServer.
Pc A søger med DiscoveryClient. Brug den fundne IP i TcpClient, eksempelvis dotnet run --project NetvaerkLabs/TcpClient -- 192.168.50.20 6000.
Erstat eksemplets IP med det faktiske svar. Send PING og forvent PONG. Et discovery-svar alene beviser ikke, at TCP-tjenesten kan kontaktes.


## Gruppen og det lokale interface

Modtageren binder til UDP-porten og melder sig ind i gruppen. localInterface er en adresse på modtagerens egen pc.
Senderen bruger samme gruppe og port. TTL (Time To Live) på 1 begrænser IP-hop, men erstatter ikke netværkets adgangsregler.

```csharp
receiver.JoinMulticastGroup(group, localInterface);

sender.MulticastLoopback = true;
sender.Ttl = 1;
await sender.SendAsync(bytes,
    new IPEndPoint(IPAddress.Parse("239.255.42.99"), 6300));
```


## Lokal multicast-kontrol

Start modtageren før senderen. Argumentet 127.0.0.1 vælger loopback-interface, så eksemplet først kan undersøges lokalt.
På et tilladt LAN bruger hver pc sin egen aktive IPv4-interfaceadresse. Loopback-testen virker ikke på alle systemer. Hvis der ikke kommer svar, brug gruppefiguren og fortsæt.

```csharp
dotnet run --project NetvaerkLabs/MulticastReceiver -- 127.0.0.1 6300

dotnet run --project NetvaerkLabs/MulticastSender -- 127.0.0.1 6300
```


## Rød · Afprøv multicast med forberedt kode

Start MulticastReceiver med loopback-argumentet fra kommandoeksemplet ovenfor. Kør MulticastSender, og se efter HEJ GRUPPE i modtagerens vindue.
Prøv eventuelt to modtagere på samme pc. Portdeling er forberedt, men den konkrete platform og netværket kan begrænse resultatet.
Hvis testen blokeres, brug gruppeillustrationen til at følge forløbet og fortsæt. Broadcast/multicast erstatter ikke den gennemførte unicast-øvelse.


## Gul · Ændr kontrakten og test igen

Tilføj en regel i TCP-serverens svarvalg: når linjen er VERSION, skal svaret være VERSION 1. Behold PING/PONG og ECHO.
Genstart serveren, og send VERSION fra clienten. Forvent VERSION 1. Test derefter PING og en almindelig tekst igen.
Clientens transportkode skal fortsat kunne sende og modtage linjer. Den nye betydning af VERSION ligger i applikationsprotokollen.

