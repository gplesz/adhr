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
  - ha megvan a csomópont, akkor egy áttekintő/müdosító abléak, ami csak a releváns mezőket tartalmazza.

# Egyéb
- az alkalmazásnak képesnek kell lenni arra, hogy
- atributumcsoportokkal dolgozzon, ami felhasználócsoporthoz tartozzon
- és legyen felkészülve arra, hogy nincs minden mezőhöz írási/olvasási joga.
- csak rész: adatexport értelmes módon (karácsonyi címlista, stb.)

# Felépítés/architektúra
- LDAP/Powershell: ebből az LDAP felületet választjuk (ha mindent elérünk, ami kell)
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

