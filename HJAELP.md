# Hjælp til TCP og UDP

Brug afsnittet til den test, I arbejder med, og vend tilbage til øvelsen. Komplette støttefiler findes også i Skabeloner.

## Løsning · TCP-clientens udveksling

Metoden skriver én linje og læser én svarlinje. Tokenet kommer fra den forberedte femsekundersfrist.

```csharp
await writer.WriteLineAsync(line.AsMemory(), token);
return await reader.ReadLineAsync(token);
```


## Løsning · TCP-serverens beskedløkke

En null-værdi afslutter løkken. Hver læst linje giver ét svar efter vores kontrakt.

```csharp
string? line;
while ((line = await reader.ReadLineAsync()) is not null)
{
    Console.WriteLine($"Modtaget: {line}");
    string reply = line == "PING" ? "PONG" : $"ECHO {line}";
    await writer.WriteLineAsync(reply);
}
```


## Løsning · UDP-senderens datagram

En tekst bliver til bytes og sendes i ét UDP-datagram. Kontrollen af modtagelse foregår i modtagerens konsol.

```csharp
byte[] bytes = Encoding.UTF8.GetBytes(message);
await sender.SendAsync(bytes, endpoint);
```


## Fejl: ventende TCP-svar

Hvis TCP er forbundet, men der ikke kommer svar, skal I først kontrollere linjeskiftet og AutoFlush.
Kontrollér derefter, om den sekventielle server stadig håndterer en anden åben client. Clientens timeout kan blive nået, mens den venter.
Efter rettelse skal I genstarte server og client. En lukket eller udløbet forbindelse bruges ikke igen.


## Start og stop i Visual Studio

Brug projektets Debug → Start Without Debugging. Stop serveren eller modtageren med Ctrl+C i dens eget konsolvindue. Programmer startet uden debugger kan fortsat køre, selv om Visual Studio ikke viser en aktiv debugging-session. Stop dem før rettelser og nyt build. Terminaler er kun et alternativ; kerneøvelserne kræver ikke PowerShell.

## Fejl: adresse eller port er optaget

Address already in use betyder, at en anden listener allerede bruger den pågældende binding. Stop den tidligere server eller modtager, før I starter en ny.
Hvis buildet ikke kan erstatte en DLL-fil, kan det gamle program stadig køre. Stop det med Ctrl+C. Et PID i fejlteksten kan findes i Joblistes Detaljer.
PID betyder Process ID. Afslut kun den konkrete verificerede proces. Andre programmer kan også hedde dotnet.exe.


## Fejl: ingen UDP-beskeder

Kontrollér modtagerens port, dens binding og senderens destinationsadresse. Modtageren skal være startet før afsendelsen.
Ved broadcast og multicast skal interface, netværksregler og eventuelt gruppemedlemskab også passe. En vellykket lokal unicast-test beviser ikke, at gruppetrafikken virker.
Manglende beskeder og en frist uden svar giver information om testen, men beviser ikke alene, hvilken del der fejlede.

