# 2025-p3a-webgame-bursagroh
2025-p3a-webgame-bursagroh created by GitHub Classroom

## Klíčové nápady
- RPG
- 2D
- V opuštěném městě
- Rekonstruované centrální budovy s nějakou funkcionalitou
- Random generované opuštěné budovy (náhodný počet pater, náhodná mostra, náhodný loot)

## Centrální budovy
- Centrální market = obchodování s ostatními hráči
- Banka = uschovna peněz a věcí
- Restaurace = vydělávání peněz pomocí minihry
- Důl = dělání suroviny
- Kovárna = místo na crafting

### Centrální market
Místo kde hráč položí nabídku na prodej či nákup za určitou cenu. Ostatní hráči mohou nabídky příjmout a uskutečnit trade.

### Banka
Uložiště pro peníze a věci, které se neztratí po smrti hráče.

### Restaurace
Místo kde si hráč mohou vydělat peníze, třeba na první meč, pomocí minihry (skládání burgerů podle předlohy).

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

### Combat
Hráč nebo enemák táhne první a pak se střídají.
Možné akce:
- Útok pěstí
- Útok zbraní
- Odpočinek
- Použití itemu (např. heal)
Když je hráč napaden objeví se timer kdy může hráč uhnout, blokovat nebo nic.

### Gameplay v centrálním marketu
Jenom UI obchod, kde hráč může vystavit nabídku nebo poptávku. Dále hráč může vystavené nabídky příjmout.

### Gameplay v bance
Jenom UI s inventářem hráče a banky. Hráč může libovolně přetahovat itemy, pokud se mu vejdou do inventáře.

### Gameplay v restauraci
Hráč dostane instrukce na sestavení burgeru. Hráč nakliká zadaný burger, když to nesplete, tak dostane malou peněžní odměnu.

### Gameplay v dole
Náhodně se vygeneruje grid 8 široký s nekonečnou hloubkou s orečkama, hráč může orečko vytěžit. Dál v dole budou vzácnější suroviny.
Hráč na vykopání potřebuje krumpáč, který si může za peníze vypůjčit nebo vycraftit v kovárně.

### Gameplay v kovárně
Jenom UI kde si hráč může vycraftit itemy ze surovin. Také si hráč musí zakoupit plánek na jednotlivé itemy.
