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

