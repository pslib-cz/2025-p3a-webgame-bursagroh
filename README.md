# Urban Relic
Odkaz na figmu: https://www.figma.com/design/Mtsedc7KYgaIreFrquDKqu/bursagroh-team-library?node-id=3313-2&t=raORM9mkjraROVin-1

## Popis hry

Hra je 2D RPG zasazené do opuštěného města, kde se hráč jako poslední přeživší snaží přežít v prostředí plném monster a nebezpečí. Hlavním cílem je získat vzácné suroviny a vykovat legendární meč, který následně přinese do fontány, což znamená vítězství ve hře.

### Herní svět

Město je rozděleno do pravidelného gridu, po kterém se hráč pohybuje kliknutím na jednotlivá políčka. Ve městě se nacházejí dva typy budov:

**Centrální (bezpečné) budovy:**
- **Restaurace** - Zde hráč vydělává peníze pomocí minihry (skládání burgerů). Toto je první místo, kam by měl hráč zajít, protože na začátku nemá žádné peníze ani vybavení. Minihra může být opakována neomezeně.
- **Banka** - Bezpečné uložiště pro peníze a předměty. Vše, co je v bance, se neztratí při smrti hráče.
- **Důl** - Místo pro těžbu surovin. Hráč potřebuje krumpáč (lze vypůjčit za peníze nebo vycraftit). Důl má nekonečnou hloubku a čím hlouběji hráč kopá, tím vzácnější suroviny najde.
- **Kovárna** - Zde hráč vyrábí předměty ze surovin podle plánků (blueprintů), které si musí nejdříve zakoupit.
- **Fontána** - Místo, kam hráč musí přinést legendární meč pro vítězství.

**Opuštěné (nebezpečné) budovy:**
- Náhodně generované budovy s různým počtem pater (8x8 políček každé patro)
- Obsahují monstra, loot chestky a na nejvyšším patře bosse s hodnotnou truhlou
- Běžné budovy lze kdykoliv opustit
- **Trap budovy** (označené červeně) se po vstupu uzamknou a lze je opustit pouze po zabití bosse, ale nabízejí lepší odměny

### Herní mechaniky

**Průzkum budov:**
- Hráč se pohybuje po 8x8 gridu jen na sousední políčka
- Na políčkách může narazit na: schody, chestky, předměty nebo monstra
- Chestky se otevřou automaticky a jejich obsah vypadne do okolí
- Monstra se po každé akci hráče pohybují směrem k němu
- Když se hráč a monstrum setkají, začíná combat

**Combat systém:**
- Tahová soubojová mechanika (střídají se hráč a nepřítel)
- Možné akce: útok pěstí, útok zbraní, odpočinek, použití předmětu (např. healing potion)
- Při nepřátelském útoku se zobrazí timer, během kterého může hráč uskočit, blokovat nebo nic
- Po porážce monstra z něj vypadne loot
- Bosové po porážce zanechají speciální loot chestku

**Inventář a management:**
- Hráč má omezený inventář podle kapacity
- Předměty mají hmotnost a durabilitu
- Při smrti hráč ztrácí celý inventář (kromě věcí uložených v bance)
- Po smrti lze respawnout, ale bez jakýchkoliv předmětů

**Progrese:**
1. Vydělat peníze v restauraci
2. Pronajmout si krumpáč a těžit suroviny v dole
3. Koupit plánky a vycraftit lepší vybavení v kovárně
4. Prozkoumat nebezpečné budovy a získat vzácný loot
5. Získat všechny potřebné materiály pro vytvoření legendárního meče
6. Přinést meč do fontány a vyhrát

### Ukládání hry

Hra používá unikátní systém ukládání bez nutnosti registrace:
- Při prvním připojení získá hráč unikátní ID
- Toto ID lze zobrazit a uložit si ho
- Při načtení ID se data zkopírují do aktuální session
- Více hráčů může začít ze stejného save pointu
- Data jsou uložena na serveru a nelze je měnit

### Strategie přežití

- **Smrt má své následky** - Při smrti ztratíte vše v inventáři, ukládejte důležité věci do banky
- **Trap budovy jsou riziková zóna** - Červené budovy nelze opustit bez porážky bosse, vstupujte jen dobře připravení
- **Lepší vybavení = vyšší přežití** - Nespěchejte do velkých soubojů bez kvalitních zbraní a lektvarů
- **Crafting je klíčový** - Sbírejte vzácné materiály pro vytvoření legendárního meče

## Rozdělení
Šimon Bursa - backend
Filip Groh - frontend
Jakub Procházka - výpomoc s assety

---

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
Na mapě budou centrální budovy. Neobjevené budouvy budou označeny prázdně, pokud trap budova, tak bude označena červeně. Když hráč navštíví budovu, tak se označí na mapě čislem maximálního navštíveného patra (2/?), když hráč dorazí do boss patra, tak se změní na (5). Když hráč zabije bosse, tak se označení změní na křížek a číslo.

### Gameplay v budově
Patro je 8x8 grid, kde se hráč může pohnout pouze na sousedící políčko.
Hráč vejde do budovy, pokud se jedná o trap budovu, tak se uzamkne východ a nebude moci odejít, pokud se nejedná o trap budovu, tak může odejít kdy chce. Hráč může přejít z patra do patra pomocí schodů i když jsou na patře enemáci.
Když se hráč pohne na políčko schody, tak se přesune do příslušného patra. Pokud se hráč přesune na políčko s loot chestkou, tak se otevře a itemy náhodně vypadnou v okolí chestky. Pokud se hráč přesune na políčko s itemem, tak ho může zvednout. Když hráč dojde na políčko s enemákem spustí se combat, po poražení enemáka z něj vypadne loot item. Hráč ve svém kole může také použít item (např. healing potion). Po každé akci se enemáci přesunou blíže ke hráči, pokud se dojdou na stejné políčko jako hráč, nastane combat fáze.

### Gameplay v restauraci
Minihra bude spočívat ve skládání burgerů - server vybere předgenerovaný burger z databáze a úkol hráče bude jej poskládat, za což bude odměněn. Nejlepší výsledky se budou ukládat a budou veřejně dostupné.
Minihra bude moci být opakována do nekonečna - ze začátku totiž hra počítá, že hráč nemá žádné peníze nebo itemy - restaurace bude tedy první možnost si peníze vydělat.

### Gameplay v bance
Jenom UI s inventářem hráče a banky. Hráč může libovolně přetahovat itemy, pokud se mu vejdou do inventáře.

### Gameplay v dole
Náhodně se vygeneruje grid 8 široký s nekonečnou hloubkou s orečkama, hráč může orečko vytěžit.
Hráč na vykopání potřebuje krumpáč, který si může za peníze vypůjčit nebo vycraftit v kovárně.

### Gameplay v kovárně
Jenom UI kde si hráč může vycraftit itemy ze surovin. Také si hráč musí zakoupit plánek na jednotlivé itemy.

### Combat
Hráč nebo enemák táhne první a pak se střídají.
Možné akce:
- Útok pěstí
- Útok zbraní
- Použití itemu (např. heal)

## Ukládání
Když se hráč poprve napojí, tak obdrží nějaké ID, pak hráč bude hrát a měnit stav na serveru pod tímto ID. Hráč si toto ID může zobrazit a uložit. Ale když hráč načte toto ID, tak se jen zkopírují data uložená pod tímto ID do aktuálního ID. Tímto způsobem můžeme ukládat data na serveru, tak aby si je hráči nemohli upravovat a umožníme hráčům si ukládat herní stav bez nutnosti přihlášení. Také si více hráčů může načíst 1 ID a pokračovat ze stejnoho starting pointu.
