![](https://pedellus.visualstudio.com/_apis/public/build/definitions/16e7611b-3a78-4b03-bed9-9a04deef2d3a/14/badge)

# adhr: mezei felhasználók számára az Active Directory megszelidítése

Szükség van egy olyan alkalmazásra:

- hozzáfér az Active Directory adatbázisához
- rögzíthet kapcsolattartókat (contact)
- azokat a mezőket, amik a felhasználó számára engedélyezettek, írhatja, olvashatja a felhasználók és a contact adatokban.
- a célközönség:
  - HR munkatárs,
  - titkárnő (névjegykártyák rögzítése)
  - stb.


# Formai követelmények
- legyen nagyon egyszerű
  - egy darab kereséssel, vagy listával kiválasztani a felhasználót/contact-ot
  - ha megvan a csomópont, akkor egy áttekintő/módosító ablak, ami csak a releváns mezőket tartalmazza.

# Egyéb
az alkalmazásnak képesnek kell lenni arra, hogy
- atributumcsoportokkal dolgozzon, (attributum = mező innentől kezdve)
- felhasználócsoportonként különböző mezőkkel
- és legyen felkészülve arra, hogy nincs minden mezőhöz írási/olvasási joga.
- képes legyen adatexportra értelmes módon (karácsonyi címlista, stb.)

# Felépítés/architektúra
- LDAP/Powershell: ebből az LDAP felületet választjuk 
  persze csak akkor, ha találunk egy olyan [LDAP könyvtárat](https://long2know.com/2017/06/net-core-ldap/), amivel mindent elérünk, ami kell.
  [Itt még van néhány](http://nugetmusthaves.com/Tag/ldap) lehetséges jelentkező
  
  Mivel C# programot írunk, Powershell parancsokat kiadni egy futó programból nem olyan kényelmes. Valami ilyesmit [azért lehet tenni](https://blogs.msdn.microsoft.com/kebab/2014/04/28/executing-powershell-scripts-from-c/):
  ```CSharp
  using (var runspace = RunspaceFactory.CreateRunspace())
  {
      using (var powerShell = PowerShell.Create())
      {
          powerShell.Runspace = runspace;
          powerShell.AddScript(PsScript);
          powerShell.Invoke();
      }

      foreach (var thingy in thingies)
      {
          using (var powerShell = PowerShell.Create())
          {
              powerShell.Runspace = runspace;

              powerShell.AddCommand("My-Function");
              powerShell.AddParameter("ParamA", varA);
              powerShell.AddParameter("ParamB", varB);
              powerShell.AddParameter("ParamC", varC);
              powerShell.AddParameter("ParamD", varD);
  
              var results = powerShell.Invoke();

              // Do whatever with results
          }
      }
  }
  ```
  Ezen kívül az LDAP egy olyan szabvány, amivel más LDAP szerverhez is tudunk csatlakozni esetleg, ez a Powershell-lel nem menne ilyen egyszerűen.

- LDAP/beépített objektumok: LDAP egy szabvány, jobban járunk, ha ezt használjuk.
  
- Az Attributumok karbantartása (felvétel, módosítás, jogosultságok, stb.) NEM az alkalmazás feladata.
- Jogosultsági fogatókönyv szempontjából talán jobb, ha dedikált gépekről futnak az LDAP kérések, és a felhasználó felé egy másik felületet ad. Viszont, ezzel elveszítjük az AD elosztott jellegét, így jobb lenne, ha minden felhasználónak saját alkalmazása lenne, és ez a legközelebbi tartományvezérlőhöz csatlakozik.
- Főbb elemei az alkalmazásnak:
  - LDAP konnector (ez intézi a kéréseket és fogadja a válaszokat az AD-hez/től)
  - Szervíz, ami elvégzi a feladatokat
  - felhasználói felület
  - kezdetben egyben tartjuk az alkalmazást, és desktop alkalmazást írunk.

# Szereplők
- Rendszergazda
- Üzemeltető
- Mezei felhasználó (titkárnő/hr-es/stb.)

# Ezzel foglalkozó tanfolyam
https://app.netacademia.hu/Tanfolyam/2018wu05-ad-sema-bovitese

# Facebook csoport
https://www.facebook.com/groups/dotnetcapak

# LDAP könyvtárak
- https://www.nuget.org/packages/Novell.Directory.Ldap.NETStandard/
- https://github.com/dsbenghe/Novell.Directory.Ldap.NETStandard

- https://github.com/wedoit-io/LDAP-Library
  ennek a nuget csomagja: https://www.nuget.org/packages/LDAPLibrary/

- https://long2know.com/2017/06/net-core-ldap/
- https://stackoverflow.com/questions/37330705/working-with-directoryservices-in-asp-net-core

# Platform
- A multiplatformot elvetjük, desktop alkalmazás készítéséhez nem megfelelő a támogatás még. Egyszer windows-os WPF alkalmazást írunk, a ClickOnce telepítő pedig segít a problémamentes telepítésben.

- a novell könyvtár egy kicsit kidolgozottabbnak néz ki, így azzal kezdem a munkát. Kell egy repository, ami a CRUD műveleteket elvégzi.

https://github.com/dsbenghe/Novell.Directory.Ldap.NETStandard
https://www.nuget.org/packages/Novell.Directory.Ldap.NETStandard

Tehát ez:

```
Install-Package Novell.Directory.Ldap.NETStandard -Version 2.3.8
```

[Active directory modul telepítése](https://serverfault.com/a/693331) a szerveren
```
Import-Module ServerManager
Install-WindowsFeature RSAT-AD-PowerShell
```

Telepítenünk kell az [LDAP using AD LDS] szolgáltatást(https://blogs.msdn.microsoft.com/mlserver/2017/04/10/step-by-step-guide-to-setup-ldaps-on-windows-server/)

[MICROSOFT REMOTE SERVER ADMINISTRATION TOOLS FOR WINDOWS 10](https://www.microsoft.com/en-us/download/confirmation.aspx?id=45520)

Megyünk az active directory felé:

https://www.nuget.org/packages/System.DirectoryServices.AccountManagement/

ActiveDirectory: a felhasználó jogainak olvasása (írási/olvasási jogosultságok)
ActiveDirectory: saját property rögzítése, és írhatóvá tétele
ActiveDirectory: titkárnő tudjon felvinni contact-ot

## Következő cél
- tesztkörnyezet 
  - Hr felhasználó alacsony jogosultságokkal
  - Titkárnő felhasználóra
    - jogosultság kell, hogy Contact adatot tudjon felvinni
  - Szükségem van legalább egy olyan adatra az ad-ben, amihez írási jogot adunk a hr felhasználónak

- bejelentkezés választható legyen:
  - domain+username+password
  - Windows authentikáció, a bejelentkezett felhanszálót használva
  - esetleg windows authentikáció + szerver vagy domain név

- A programból csak azokat a mezőket mutassuk meg, amihez a felhasználónak 
  van írási joga (ehhez le kell tudni kérdezni attributumonként az írási 
  jogot)

 ```
+------------------------+      +------------------------+                  +---------------------------+
|                        |      |                        |                  |                           |
| WPF desktop alkalmazás |      |  Repository            |                  |                           |
|                        |      |                        |                  | Active Directory Server   |
|                        |      |       CRUD             |                  |                           |
|                        | +--> |       Create           |                  |                           |
|                        |      |       Read             |                  |                           |
|                        |      |       Update           +----------------> |                           |
|                        |      |       Delete           |                  |                           |
|                        |      |       List             |                  |                           |
|                        |      |                        |                  |                           |
|                        |      |                        |                  |                           |
|                        |      |                        |                  |                           |
|                        |      |                        |                  |                           |
|                        |      |                        |                  |                           |
+------------------------+      +------------------------+                  |                           |
                                                                            |                           |
                                                                            +---------------------------+
```

### Következő lépések

- bejelentkezéskor névfeloldás
- async működés, hogy reszponzív maradjon
- csak azoknak a csomópontoknak a lekérdezése, amihez tartozik írási jog
- csak azoknak a kártyáknak a megmutatása, amihez van írható mező
- megmutatni a felhasználót, akivel be vagyunk lépve
- ha nincs kitöltve a szervernév, akkor az automatikus szervert keresse
- bejelentezéskor névfeloldás
- nem kell létrehozás és törlés
- 

