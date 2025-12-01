# 2025-p3a-webgame-bursagroh
Odkaz na figmu: https://www.figma.com/design/Mtsedc7KYgaIreFrquDKqu/bursagroh-team-library?node-id=3313-2&t=raORM9mkjraROVin-1

## Rozdělení
Šimon Bursa - backend

Filip Groh - frontend

## Klíčové nápady
- RPG
- 2D
- V opuštěném městě
- Rekonstruované centrální budovy s nějakou funkcionalitou
- Random generované opuštěné budovy (náhodný počet pater, náhodná mostra, náhodný loot)

## Centrální budovy
- Banka = uschovna peněz a věcí
- Restaurace = vydělávání peněz pomocí minihry
- Důl = dělání suroviny
- Kovárna = místo na crafting itemů


### Banka
Uložiště pro peníze a věci, které se neztratí po smrti hráče.

### Restaurace
Místo kde si hráč mohou vydělat peníze, třeba na první meč, pomocí minihry.

### Důl
Místo kde si hráč může vypůjčit krumpáč a pak s ním jít těžit suroviny.

### Kovárna
Místo kde hráč pomocí natěžených / nalezených surovin může vycraftit itemy.

## Opuštěné budovy
Náhodně generované budovy, kde se náhodně budou spawnovat nepřátelé a loot, v posledním patře boss a loot chestka.
Budovy jsou různě vysoké a hráči neví jak moc. Boss vždy v nejvyšším patře, ale hráči nemusí vyčistit celou budovu, také hráči mohou opustit budovu i když nezabijí bosse. Když hráč vejde do patra s bossem, tak ho bude muset porazit, aby mohl odejít.
Také možnost trap budov, které se uzamknou poté co hráč vejde a odemkne se po zabití bosse. Tyto budovy budou označeny a bude v nich lepší loot a silnější monstra.
Každé patro bude mít schody nahoru a schody dolu pokud není v přízemí nebo v patře s bossem (to nejvyšší). Dále tam budou loot chestky a monstra.

## Gameplay
### Gameplay na mapě
Mapa je rozdělena do pravidelného gridu budov. Hráč se pohybuje clickem na políčka nebo budovy, pokud clickne na budovu, tak se přesune dovnitř.
Na mapě budou označeny centrální budovy ikonama. Neobjevené budouvy budou označeny prázdně, pokud trap budova, tak bude označena červeně. Když hráč navštíví budovu, tak se označí na mapě čislem maximálního navštíveného patra (2/?), když hráč dorazí do boss patra, tak se změní na (5). Když hráč zabije bosse, tak se označení změní na křížek a číslo. Hráč také může použít mapu před vstupem, pro zjištění počtu pater.

### Gameplay v budově
Patro je 8x8 grid, kde se hráč může pohnout pouze na sousedící políčko.
Hráč vejde do budovy, pokud se jedná o trap budovu, tak se uzamkne východ a nebude moci odejít, pokud se nejedná o trap budovu, tak může odejít kdy chce. Hráč může přejít z patra do patra pomocí schodů i když jsou na patře enemáci, toto neplatí u boss patra, kde hráč musí nejdříve porazit bosse aby mohl odejít.
Když se hráč pohne na políčko schody, tak se přesune do příslušného patra. Pokud se hráč přesune na políčko s loot chestkou, tak se otevře a itemy náhodně vypadnou v okolí chestky. Pokud se hráč přesune na políčko s itemem, tak ho může zvednout. Když hráč dojde na políčko s enemákem spustí se combat, po poražení enemáka z něj vypadne loot item, pokud se jedná o bosse vypadne boss loot chestka. Hráč ve svém kole může také použít item (např. healing potion). Po každé akci se enemáci přesunou blíže ke hráči, pokud se dojdou na stejné políčko jako hráč, nastane combat fáze a hráčova akce se vyruší.

### Gameplay v restauraci
Minihra bude spočívat ve skládání burgerů - server vybere předgenerovaný burger z databáze a úkol hráče bude jej poskládat, za což bude odměněn. Nejlepší výsledky se budou ukládat a budou veřejně dostupné - nejlepší čas bude odměněn ještě lépe.
Minihra bude moci být opakována do nekonečna - ze začátku totiž hra počítá, že hráč nemá žádné peníze nebo itemy - restaurace bude tedy první možnost si peníze vydělat.

### Gameplay v bance
Jenom UI s inventářem hráče a banky. Hráč může libovolně přetahovat itemy, pokud se mu vejdou do inventáře.

### Gameplay v dole
Náhodně se vygeneruje grid 8 široký s nekonečnou hloubkou s orečkama, hráč může orečko vytěžit. Dál v dole budou vzácnější suroviny.
Hráč na vykopání potřebuje krumpáč, který si může za peníze vypůjčit nebo vycraftit v kovárně.

### Gameplay v kovárně
Jenom UI kde si hráč může vycraftit itemy ze surovin. Také si hráč musí zakoupit plánek na jednotlivé itemy.

### Combat
Hráč nebo enemák táhne první a pak se střídají.
Možné akce:
- Útok pěstí
- Útok zbraní
- Odpočinek
- Použití itemu (např. heal)
Když je hráč napaden objeví se timer kdy může hráč uhnout, blokovat nebo nic.

## Ukládání
Když se hráč poprve napojí, tak obdrží nějaké ID, pak hráč bude hrát a měnit stav na serveru pod tímto ID. Hráč si toto ID může zobrazit a uložit. Ale když hráč načte toto ID, tak se jen zkopírují data uložená pod tímto ID do aktuálního ID. Tímto způsobem můžeme ukládat data na serveru, tak aby si je hráči nemohli upravovat a umožníme hráčům si ukládat herní stav bez nutnosti přihlášení. Také si více hráčů může načíst 1 ID a pokračovat ze stejnoho starting pointu.

## Endpointy (návrh)

### Save
GET    api/Save/{PlayerId}  
POST   api/Load/{SaveString}

### Player
GET    api/Player/Generate  
GET    api/Player/{PlayerID}  
PUT    api/Player/{PlayerID}

### Bank
GET    api/Bank/{PlayerID}  
GET    api/Bank/Item/{BankID}

### Building
GET    api/Building/{PlayerID}  
GET    api/Building/Floor/{BuildingID}

### Blacksmith
GET    api/Blacksmith/{PlayerID}  
GET    api/Blacksmith/Player/{id}  
GET    api/Blacksmith/Crafting/{id}

### Recipe
GET    api/Recipe/{id}  
GET    api/Recipe/Ingredience

### Mine
GET    api/Mine/{MineID}  
GET    api/Mine/{MineID}/Data





