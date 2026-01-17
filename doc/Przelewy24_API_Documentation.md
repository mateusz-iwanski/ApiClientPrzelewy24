# System p?atno?ci

Serwis Przelewy24 prowadzi system autoryzacji i rozlicze里 oraz ?wiadczy us?ugi p?atnicze w charakterze krajowej instytucji p?atniczej.

Poprzez API Przelewy24 mo?esz uzyska? dost?p do wszystkich us?ug oferowanych przez system. Poni?sza dokumentacja zawiera najcz??ciej wykorzystywane funkcjonalno?ci. Skontaktuj si? z Dzia?em Handlowym, aby pozna? inne funkcjonalno?ci.

Aby uzyska? dost?p do API Przelewy24, w pierwszej kolejno?ci za?車? konto w Panelu Administracyjnym P24. Po dokonanej rejestracji, sprzedawca ma mo?liwo?? ?ledzenia w panelu administracyjnym stanu swojego konta, wszystkich p?atno?ci klient車w oraz wykonanych zwrot車w na bie??co.

## Przebieg transakcji

Klient po skompletowaniu zam車wienia klika na przycisk "zap?a?". System Sprzedawcy przesy?a ??danie rejestracji transakcji do systemu P24 i otrzymuje zwrotnie unikalny TOKEN. Nast?pnie klient jest przekierowany na panel transakcyjny P24.

W przypadku anulowania p?atno?ci klient jest przekierowany na adres ※urlReturn".

Po poprawnej transakcji Klient jest kierowany na adres podany w parametrze ※urlReturn§. System P24 wysy?a potwierdzenie transakcji na adres podany w parametrze "urlStatus". Notyfikacja z potwierdzeniem transakcji jest wysy?ana w spos車b asynchroniczny.

Dla potwierdzenia wiarygodno?ci otrzymanego potwierdzenia w odpowiedzi na potwierdzenie wp?aty system sprzedawcy weryfikuje wynik ??daniem zwrotnym.

## Payment process

### Wymagania programowe

Aby prawid?owo przeprowadzi? transakcj? sprzedawca na swoich stronach WWW musi wprowadzi? ni?ej opisan? obs?ug? wysy?ania ??dania transakcji oraz odbi車r odpowiedzi o wyniku transakcji.

Ca?y proces przebiega w spos車b automatyczny bez konieczno?ci ingerencji obs?ugi sklepu w proces p?atno?ci.

Po poprawnie zako里czonym procesie p?atno?ci status danego zam車wienia w sklepie powinien automatycznie zmieni? si? na zap?acone/przyj?te do realizacji. W tym momencie obs?uga sklepu mo?e przyst?pi? do realizowania zam車wienia.

## Environment

### Authentication

P24 wspiera mechanizmy Basic Authentication.

#### basicAuth

Jest to podstawowa metoda uwierzytelnienia. User i secretId dost?pne s? w panelu:
- "User" odpowiada tej samej warto?ci, co posId,
- secretId, odpowiada tej samej warto?ci co klucz do raport車w (klucz do API).

**Security Scheme Type:** HTTP  
**HTTP Authorization Scheme:** basic

### ?rodowisko produkcyjne i sandbox

#### Rejestracja konta produkcyjnego

Zarejestruj swoje konto w serwisie Przelewy24 - [link](https://www.przelewy24.pl/rejestracja)

#### Konfiguracja konta testowego (sandbox)

Skonfiguruj konto sandbox. Jedynie maj?c dost?p do serwisu produkcyjnego masz mo?liwo?? uruchomienia konta sandbox. Z menu g?車wnego wybierz 'Moje konto', a nast?pnie 'Konto w SANDBOX'.

#### Konfiguracja konta u?ytkownika i cz?onk車w zespo?u

Je?eli zachodzi potrzeba, skonfiguruj konta cz?onk車w zespo?u - pozwoli to na odr?bny dost?p do serwisu Przelewy24 z nadaniem odpowiednich r車l u?ytkownikom w zale?no?ci od potrzeb.

### Integracja API

Skonfiguruj dost?p do konta, korzystaj?c z danych uwierzytelniaj?cych API (wersja produkcyjna i sandbox serwisu Przelewy24):
- user (posId/login) - ID konta Przelewy24 - wysy?ane w mailu potwierdzaj?cym pomy?ln? rejestracj? konta
- CRC - klucz CRC u?ywany do wyliczania sign
- secretId (klucz API) - 'klucz do raport車w'.

### Testowanie po??czenia API

Korzystaj?c z endpointu TestAccess oraz danych dost?pu (user oraz secretId), przetestuj po??czenie.

### Adres IP i domy?lne Web Service

Uzupe?nij pole 'Adres IP' ('Moje konto' - 'Moje dane' - 'Dane API i konfiguracja'), pod kt車rym znajduj? si? Twoje zasoby, celem dost?pu do Web Service Przelewy24.
Domy?lne serwisy to:

- TransactionRegister
- TransactionVerify
- TransactionRefund
- PaymentsMethods
- GetTransactionBySessionId.

By w??czy? inne ni? domy?lne, wymienione w specyfikacji, pro?ba o kontakt z opiekunem klienta.

## Wytyczne API

Prosimy o wykonanie poni?szych krok車w w celu zagwarantowania sprawnego dzia?ania p?atno?ci Przelewy24.

### Zweryfikowanie konfiguracji hostingu

Zalecamy korzystanie z systemu operacyjnego oraz PHP w wersji 64 bit, ze wzgl?du na wi?ksze mo?liwo?ci obliczeniowe oraz wi?ksz? wydajno??. W celu zweryfikowania wersji systemu operacyjnego, wersji PHP mo?na wykorzysta? m.in. kod phpinfo https://www.php.net/manual/en/function.phpinfo.php#refsect1-function.phpinfo-examples

```php
<?php
  // Show all information, defaults to INFO_ALL
    phpinfo();

  // Show just the module information.
  // phpinfo(8) yields identical results.
    phpinfo(INFO_MODULES);
?>

Nast?pnie nale?y zapisa? plik jako rozszerzenie PHP i wej?? w ?cie?k? / adres URL, na kt車rym jest hostowany plik. Tak utworzony plik musi zosta? umieszczony na Pa里stwa hostingu internetowym - adres tego pliku nie powinien by? nikomu udost?pniany. PAMI?TAJ!
Po weryfikacji umieszczony plik powinien zosta? usuni?ty z serwera.

Zakresy graniczne typu INT signed / unsigned
Zakres INT signed = -2,147,483,648 do 2,147,483,647

Zakres INT unsigned = 0 do 4294 967 295

Zakres BIGINT singed = -9223372036854775808 do 9223372036854775807

Zakres BIGINT unsigned = 0 do 18446744073709551615

PAMI?TAJ!
Je?li korzystaj? Pa里stwo z OS lub PHP w wersji 32bit, to kod aplikacji mo?e interpretowa? warto?? wi?ksz? ni? (2147483647 - maksymalna warto?? INT signed) jako typ float - co mo?e spowodowa? problemy w procesowaniu p?atno?ci.

php
$large_number = 2147483648;
var_dump($large_number); // float(2147483648)
W takim przypadku nale?y zmieni? zakres na BIGINT, w celu dopuszczenia warto?ci wi?kszej ni? maksymalna warto?? zakresu INT signed.

Baza danych
Je?li przechowuj? Pa里stwo w bazie danych warto?? order_id - ID zam車wienia z systemu Przelewy24, prosimy o zweryfikowanie, czy ustawiony typ kolumny dopu?ci zapis/odczyt warto?ci wi?kszej ni? maksymalna warto?? INT signed = 2147483647.

W takim przypadku nale?y zmieni? zakres na BIGINT, w celu dopuszczenia warto?ci wi?kszej ni? maksymalna warto?? zakresu INT signed.

Rzutowanie parametru order_id
Nale?y sprawdzi?, czy Pa里stwa kod aplikacji dopu?ci do zapisu/odczytu warto?? wi?ksz? ni? maksymaln? warto?? tj. INT signed = 2147483647.

W takim przypadku nale?y zmieni? zakres na BIGINT, w celu dopuszczenia warto?ci wi?kszej ni? maksymalna warto?? zakresu INT signed

?rodowiska programistyczne
?rodowisko produkcyjne
Ka?de ??danie rozr車?nione jest swoim w?asnym, unikalnym adresem URL. W ten spos車b system P24 wie, z kt車rej funkcji API chcesz skorzysta?. W po??czeniu z bazowym adresem URL, dla za r車wno produkcyjnego jak i testowego ?rodowiska, otrzymasz kompletny adres API-URL.

Bazowy URL systemu produkcyjnego:
https://secure.przelewy24.pl/api/v1

Transakcje produkcyjne b?d? widoczne w panelu:
https://panel.przelewy24.pl/index.php

?rodowisko testowe
Podczas implementowania mechanizm車w w Twoim systemie mo?esz skorzysta? ze ?rodowiska testowego. ?rodowisko to umo?liwia zweryfikowanie poprawno?ci instalacji bez konieczno?ci dokonywania przelew車w.

Adresy URL do po??cze里 do ?rodowiska testowego:
https://sandbox.przelewy24.pl/api/v1

Transakcje testowe b?d? widoczne w panelu testowym:
https://sandbox.przelewy24.pl/panel/index.php

?rodowisko testowe nie mo?e by? wykorzystywane do realizacji transakcji produkcyjnych.

Adresy IP serwer車w
Zalecamy zabezpieczenie skrypt車w przed podejrzanymi wywo?aniami, stosuj?c filtracj? adres車w IP dla przychodz?cych po??cze里. Zakresy IP serwer車w Przelewy24 to:

5.252.202.255 , 5.252.202.254

20.215.81.124

Wymagania ?rodowiskowe
Transport Layer Security - TLS 1.2 (wymagane minimum)

https://wiki.mozilla.org/Security/Server_Side_TLS

https://en.wikipedia.org/wiki/Transport_Layer_Security

OpenSSL 1.0.1 (wymagane minimum)

https://www.openssl.org/news/changelog.html#x31

cURL 7.34.0

https://curl.haxx.se/docs/manpage.html#--tlsv12

Mo?liwe kody b??d車w
ErrorCode	Opis
err00	Nieprawid?owe wywo?anie skryptu
err01	Nie uzyskano od sklepu potwierdzenia odebrania odpowiedzi autoryzacyjnej
err02	Nie uzyskano odpowiedzi autoryzacyjnej
err03	To zapytanie by?o ju? przetwarzane
err04	Zapytanie autoryzacyjne niekompletne lub niepoprawne
err05	Nie uda?o si? odczyta? konfiguracji sklepu internetowego
err06	Nieudany zapis zapytania autoryzacyjnego
err07	Inna osoba dokonuje p?atno?ci
err08	Nieustalony status po??czenia ze sklepem.
err09	Przekroczono dozwolon? liczb? poprawek danych.
err10	Nieprawid?owa kwota transakcji!
err49	Zbyt wysoki wynik oceny ryzyka transakcji.
err51	Nieprawid?owe wywo?anie strony
err52	B??dna informacja zwrotna o sesji!
err53	B??d transakcji !
err54	Niezgodno?? kwoty transakcji!
err55	Nieprawid?owy kod odpowiedzi!
err56	Nieprawid?owa karta
err57	Niezgodno?? flagi TEST!
err58	Nieprawid?owy numer sekwencji!
err59	Nieprawid?owa waluta transakcji!
err101	B??d wywo?ania strony W ??daniu transakcji brakuje kt車rego? z wymaganych parametr車w lub pojawi?a si? niedopuszczalna warto??.
err102	Min?? czas na dokonanie transakcji
err103	Nieprawid?owa kwota przelewu
err104	Transakcja oczekuje na potwierdzenie.
err105	Transakcja dokonana po dopuszczalnym czasie
err161	??danie transakcji przerwane przez u?ytkownika Klient przerwa? procedur? p?atno?ci wybieraj?c przycisk "Powr車t" na stronie wyboru formy p?atno?ci.
err162	??danie transakcji przerwane przez u?ytkownika Klient przerwa? procedur? p?atno?ci wybieraj?c przycisk "Rezygnuj" na stronie z instrukcj? p?atno?ci.
Przypadki u?ycia
Jak wy?wietli? w sklepie pe?en wyb車r metod p?atno?ci?
Aby upro?ci? proces p?atno?ci, mo?liwe jest przeniesienie wyboru formy p?atno?ci przez klienta ju? na etapie sk?adania zam車wienia w sklepie. Je?eli dodatkowo w sklepie klient zaakceptuje warunki regulaminu Przelewy24 (w ??daniu nale?y ustawi? regulationAccept = true), zostanie on po klikni?ciu przycisku ?zap?a?§, przeniesiony bezpo?rednio ze strony sklepu do banku / formularza kart p?atniczych. Na stronie sklepu nale?y umie?ci? i wy?wietli? klientowi nast?puj?c? tre??: ?O?wiadczam, ?e zapozna?em si? z regulaminem i obowi?zkiem informacyjnym serwisu Przelewy24§. Pod s?owem regulamin i obowi?zek informacyjny musi by? link do stron z tymi dokumentami. Checkbox nie mo?e by? odg車rnie zaznaczony.

Aby pobra? liste p?atno?ci, skorzystaj z metody PaymentMethods, opisanej w Dodatkowych Us?ugach.
Pobran? list? mo?na w dowolny spos車b zaprezentowa? na swojej stronie.

Jak przekierowa? klienta do konkretnej metody p?atno?ci?
W celu przekierowania klienta bezpo?rednio do wybranej metody p?atno?ci, nale?y przekaza? identyfikator danej metody w polu method w ??daniu rejestracji transakcji. Dla przyk?adu, przy przekierowaniu do metody mTransfer, ??danie wygl?da w ten spos車b:

json
{
  "merchantId": {{merchantId}},
  "posId": {{posId}},
  "sessionId": "{{sessionId}}",
  "amount": {{amount}},
  "currency": "{{currency}}",
  "description": "{{description}}",
  "email": "{{email}}",
  "country": "PL",
  "language": "pl",
  "method": {{method}},
  "urlReturn": "{{urlReturn}}",
  "sign": "{{sign}}"
}
Jak ograniczy? klientowi czas dost?pny na zrealizowanie p?atno?ci?
W zale?no?ci od specyfiki danego systemu, mo?e zachodzi? potrzeba ograniczenia czasu, jaki klient ma na zrealizowanie p?atno?ci. Do sterowania tym elementem s?u?y parametr timeLimit. Ustawienie tego parametru na warto?ci z zakresu 1 - 99 okre?li limit czasu w minutach. Ustawienie parametru na 0 oznacza brak limitu.

Jak umo?liwi? p?ynny powr車t klienta do sklepu, bez konieczno?ci oczekiwania na synchroniczne potwierdzenie p?atno?ci?
W przypadku niekt車rych metod p?atno?ci, w szczeg車lno?ci e-przelew車w, wykonana p?atno?? zostaje potwierdzona w ci?gu kilku minut. Istnieje mo?liwo?? "pozostawienia" klienta w serwisie transakcyjnym w celu oczekiwania na wynik transakcji i przekierowania go z powrotem do sklepu dopiero po otrzymaniu potwierdzenia (w ten spos車b sklep b?dzie ju? posiada? potwierdzenie p?atno?ci) lub mo?na od razu przekierowa? klienta do sklepu, bez oczekiwania na wynik transakcji. Wyb車r jednego z dw車ch wariant車w jest sterowany parametrem waitForResult. Wariant pierwszy wymaga ustawienia tego parametru na "true", wariant drugi na "false".

Jak zrealizowa? zwrot transakcji do klienta?
Realizacja zwrot車w, jak wszystkie inne us?ugi w Przelewy24 jest w pe?ni automatyczna i realizowana jest poprzez narz?dzie w panelu administracyjnym Przelewy24 lub przez metod? transaction/refund.

Do jednej transakcji mo?na zleci? wiele ??da里 zwrotu, jednak sumaryczna warto?? zwrot車w nie mo?e przekroczy? pierwotnej warto?ci transakcji.

Czy po wyga?ni?ciu sesji klient mo?e doko里czy? proces p?atno?ci?
W sytuacji, gdy klient porzuci proces p?atno?ci, np. po przej?ciu na stron? banku, aby u?atwi? mu doko里czenie transakcji system Przelewy24 oferuje mo?liwo?? automatycznego wys?ania do klienta maila z linkiem do doko里czenia rozpocz?tego procesu. Je?eli klient skorzysta z tej opcji z punktu widzenia sklepu nie b?dzie r車?ni?o si? to niczym od transakcji zrealizowanej w trybie on-line.

Aby w??czy? tak? funkcjonalno?? nale?y skontaktowa? si? z opiekunem handlowym poprzez formularz kontaktowy.

Definicje
CVV 每 kod zabezpieczaj?cy karty.

Cyclic Redundancy Check (CRC) 每 unikatowy klucz (String) otrzymany od Przelewy24 s?u??cy do generowania przesy?anej sumy kontrolnej.

Dynamic Currency Conversion (DCC) 每 proces, w kt車rym kwota transakcji jest przeliczana na walut? karty p?atnika.

Merchant 每 firma lub osoba prywatna korzystaj?ca z serwisu Przelewy24.

Session ID 每 unikalny identyfikator s?u??cy do zidentyfikowania pojedynczej transakcji w systemie partnera.

Web Service 每 endpoint, protok車?, standard struktury informacji stosowany do wymiany danych mi?dzy systemami.

Materia?y graficzne
P24 logo i bannery dost?pne sa pod adresem: https://www.przelewy24.pl/do-pobrania#materialy-graficzne

Obs?uga transakcji API
Rejestracja transakcji
Authorizations: basicAuth
Request Body schema: application/json
required

Przed wys?aniem ??dania transakcji nale?y zapisa? jej dane do lokalnej bazy danych sprzedawcy. W szczeg車lno?ci nale?y zachowa? informacje o identyfikatorze sesji i kwocie transakcji.

POST - /api/v1/transaction/register

Sandbox server (uses test data)
https://sandbox.przelewy24.pl/api/v1/transaction/register

Production server (uses live data)
https://secure.przelewy24.pl/api/v1/transaction/register

Przekierowanie do panelu transakcyjnego
Adres URL https://secure.przelewy24.pl/trnRequest/{TOKEN}

gdzie {TOKEN} zosta? pobrany w wyniku zarejestrowania transakcji.

Po poprawnej transakcji zostaje wywo?ywany adres URL przekazany w procesie rejestracji transakcji w parametrze "urlStatus". Powiadomienie nast?puje niezale?nie od tego, czy Klient zosta? przekierowany na "urlReturn", czy te? nie. Powiadomienie zostaje wys?ane tylko i wy??cznie dla poprawnej wp?aty. System nie wysy?a informacji o transakcjach, kt車re nie zosta?y wykonane, b?d? zosta?y wykonane niepoprawnie. Notyfikacja wysy?ana jest w formacie JSON.

Zobacz JSON wyniku transakcji
Parameter	Typ	Opis
merchantId	required integer	ID Sklepu
posId	required integer	ID Sklepu (domy?lnie ID Sprzedawcy)
sessionId	required string <= 100 characters	Unikalny identyfikator z systemu sprzedawcy
amount	required integer	Kwota transakcji wyra?ona w groszach, np. 1.23 PLN = 123
currency	required string <= 3 characters	Warto?? zgodna z ISO np. PLN
description	required string <= 1024 characters	Opis transakcji
email	required string <= 50 characters	Email Klienta
client	string <= 40 characters	Imi? i nazwisko Klienta
address	string <= 80 characters	Adres Klienta
zip	string <= 10 characters	Kod pocztowy Klienta
city	string <= 50 characters	Miasto Klienta
country	required string <= 2 characters Default: "PL"	Kody kraj車w zgodnie ISO, np. PL, DE itp
phone	string <= 12 characters	Telefon klienta w formacie 481321132123
language	required string <= 2 characters Default: "pl"	Jeden z nast?puj?cych kod車w kraj車w zgodnie z norm? ISO 639-1: bg, cs, de, en, es, fr, hr, hu, it, nl, pl, pt, se, sk, ro
method	integer	Identyfikator metody p?atno?ci. Lista metod p?atno?ci widoczna w panelu lub dost?pna przez API
urlReturn	required string <= 250 characters	Adres powrotny po zako里czeniu transakcji
urlStatus	string <= 250 characters	Adres do przekazania statusu transakcji
timeLimit	integer	Limit czasu na wykonanie transakcji, 0 - brak limitu, maks. 99 (w minutach)
channel	integer Enum: 1 2 4 8 16 32 64 128 256 4096 8192 16384	1 - karty + ApplePay + GooglePay, 2 - przelewy, 4 - przelew tradycyjny, 8 - N/A, 16 - wszystkie 24/7 每 udost?pnia wszystkie metody p?atno?ci, 32 - u?yj przedp?at?, 64 每 tylko metody pay-by-link, 128 每 formy ratalne, 256 每 wallety, 4096 - karty, 8192 - blik, 16384 - wszystkie metody z wy??czeniem blika
Aby uruchomi? poszczeg車lne kana?y, nalezy zsumowac ich warto?ci.
Przyk?ad: przelewy i przelew tradycyjny: channel=6
waitForResult	boolean	Parametr determinuje, czy u?ytkownik zostanie przekierowany z powrotem do sklepu od razu po wykonaniu p?atno?ci, czy dopiero, gdy dotrze wynik transakcji (z potwierdzeniem p?atno?ci). Przeczytaj wi?cej
regulationAccept	boolean Default: false	Akceptacja regulaminu Przelewy24:
false 每 wy?wietl zgod? na stronie p24 (domy?lna),
true 每 akceptacja dokonana, nie wy?wietlaj.
W przypadku wysy?ania parametru ?true§, na stronie Partnera musi znale?? si? zgoda o tre?ci: ?O?wiadczam, ?e zapozna?em si? z regulaminem i obowi?zkiem informacyjnym serwisu Przelewy24§.
Pod s?owami regulamin i obowi?zek informacyjny musi by? link do stron z tymi dokumentami. Checkbox nie mo?e by? odg車rnie zaznaczony.
shipping	integer	Koszt dostawy/wysy?ki
transferLabel	string <= 20 characters	Opis pojawiaj?cy si? w tytule przelewu. Dozwolone znaki to [a-z A-Z 0-9 ?車??????里?車??????? . /\ :- ]
mobileLib	integer Value: 1	Przes?anie tego parametru jest niezb?dne przy wykorzystaniu bibliotek SDK. W mobileLib nale?y przes?a? warto?? 1, natomiast w parametrze sdkVersion nale?y wskaza? wersj? biblioteki, z kt車rej chcemy skorzysta?.
sdkVersion	string <= 10 characters	Wersja bibliotek mobilnych. Okre?la czy transakcja jest mobilna.
sign	required string <= 100 characters	Suma kontrolna parametr車w:
{"sessionId":"str","merchantId":int,"amount":int,"currency":"str","crc":"str"}
liczona z u?yciem sha384
WA?NE!:
przy wykorzystaniu funkcji json_encode nale?y doda? nast?puj?ce atrybuty
"JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES"
encoding	string <= 15 characters	System kodowania przesy?anych znak車w: ISO-8859-2, UTF-8, Windows-1250
methodRefId	string <= 250 characters	Specjalny parametr wymagany dla niekt車rych proces車w p?atno?ci, np. BLIK i Karty one-click.
cart	Array of objects (CartParameters)	Koszyk
additional	object	Zbi車r dodatkowych danych nt. transakcji i p?atnika
Responses
200 successful operation
Response Schema: application/json

json
{
  "data": object,
  "responseCode": number Default: 0
}
400 bad request
Response Schema: application/json

json
{
  "error": string Default: "Invalid input data",
  "code": number Default: 400
}
401 not authorized
Response Schema: application/json

json
{
  "error": string Default: "Incorrect authentication",
  "code": number Default: 401
}
Weryfikacja transakcji
Authorizations: basicAuth
Request Body schema: application/json

Po odebraniu powiadomienia, system Partnera powinien wykona? dodatkow? operacj? maj?c? na celu potwierdzenie przyj?cia wp?aty oraz potwierdzenie autentyczno?ci powiadomienia. Konieczne jest wykonanie weryfikacji transakcji za pomoc? metody transaction/verify.

Wa?ne! Transakcja zostaje uznana za potwierdzon? po jej weryfikacji. Je?eli klient dokona transakcji, wr車ci na stron? sprzedawcy, ale sprzedawca nie zweryfikuje transakcji, dana kwota nie zostanie przekazana sprzedawcy ani uwzgl?dniona w rozliczeniach. Pozostanie ona do dyspozycji klienta w formie przedp?aty.

Parameter	Typ	Opis
merchantId	required integer	ID Sklepu
posId	required integer	ID Sklepu (domy?lnie ID Sprzedawcy)
sessionId	required string <= 100 characters	Unikalny identyfikator z systemu sprzedawcy
amount	required integer	Kwota transakcji wyra?ona w groszach, np. 1.23 PLN = 123
currency	required string <= 3 characters Default: "PLN"	Waluta
orderId	required integer <int64>	Id zam車wienia z systemu Przelewy24
sign	required string	Suma kontrolna parametr車w:
{"sessionId":"str","orderId":int,"amount":int,"currency":"str","crc":"str"}
liczona z u?yciem sha384
WA?NE!:
przy wykorzystaniu funkcji json_encode nale?y doda? nast?puj?ce atrybuty
"JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES"
Responses
200 successful operation
Response Schema: application/json

json
{
  "data": object,
  "responseCode": number Default: 0
}
400 bad request
Response Schema: application/json

json
{
  "error": string Default: "Invalid input data",
  "code": number Default: 400
}
401 not authorized
Response Schema: application/json

json
{
  "error": string Default: "Incorrect authentication",
  "code": number Default: 401
}
PUT - /api/v1/transaction/verify

Request samples
Payload
Content type: application/json

json
{
  "merchantId": 0,
  "posId": 0,
  "sessionId": "string",
  "amount": 0,
  "currency": "PLN",
  "orderId": 0,
  "sign": "string"
}
Response samples
200

json
{
  "data": {
    "status": "success"
  },
  "responseCode": 0
}
Notyfikacja
Wynik transakcji
json
{
  "merchantId": integer,
  "posId": integer,
  "sessionId": string <= 100 characters,
  "amount": integer,
  "originAmount": integer,
  "currency": string <= 3 characters Default: "PLN",
  "orderId": integer <int64>,
  "methodId": integer,
  "statement": string,
  "sign": string
}
Suma kontrolna parametr車w:
{"merchantId":int,"posId":int,"sessionId":"string","amount":int,"originAmount":int,"currency":"string", "orderId":int,"methodId":int,"statement":"string","crc":"string"}

liczona z u?yciem sha384

WA?NE!:
przy wykorzystaniu funkcji json_encode nale?y doda? nast?puj?ce atrybuty
"JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES"

Automatyczne przekazywanie wyniku transakcji
W sytuacji, gdy pierwsze powiadomienie o wyniku transakcji nie zostanie poprawnie odebrane przez system sprzedawcy (nie wykona on prawid?owej weryfikacji), system P24 wy?le kolejne powiadomienia. Powiadomienia zostan? wys?ane po 3, 5, 15, 30, 60, 150 i 450 minutach (+/- 5 min.), chyba ?e wcze?niej nast?pi prawid?owa weryfikacja transakcji.

Parametry POST s? takie same, jak w przypadku pierwszego powiadomienia.

Wyliczanie sumy kontrolnej
Rejestracja transakcji
Poni?ej znajduj? si? fragmenty kodu dla 4 j?zyk車w programowania, prezentuj?ce prawid?owe wyliczanie sumy kontrolnej sign dla ??dania rejestracji transakcji.

Aby prawid?owo wylicza? sign, nale?y pami?ta? o poprawno?ci danych (parametry merchantId oraz crc to warto?ci pobierane z panelu Przelewy24, a pozosta?e warto?ci s? ustalane indywidualnie dla ka?dej transakcji przez sprzedawc?) oraz o rozr車?nieniu typ車w zmiennych (merchantId oraz amount to integer, pozosta?e to string).

WA?NE!
Nale?y pami?ta?, ?e sk?adowe sumy kontrolnej sign r車?ni? si? dla poszczeg車lnych ??da里 wysy?anych lub odbieranych z Przelewy24. Warto?? parametru sign, kt車r? nale?y przekaza? w ??daniu rejestracji transakcji, r車?ni si? od warto?ci sign przekazanej dla ??dania weryfikacji transakcji.

Przyk?ady wyliczania sumy kontrolnej sign dla ??dania rejestracji transakcji:

PHP

php
$params = [
    'sessionId' => 'unikalne-id-sesji', // Tutaj nale?y umie?ci? unikalne wygenerowane ID sesji
    'merchantId' => 999999, // Tutaj nale?y umie?ci? ID Sprzedawcy z panelu Przelewy24
    'amount' => 1234, // Tutaj nale?y umie?ci? kwot? transakcji w groszach, 1234 oznacza 12,34 PLN
    'currency' => 'PLN', // Tutaj nale?y umie?ci? walut? transakcji
    'crc' => 'crc-z-panelu-p24', // Tutaj nale?y umie?ci? pobrany klucz CRC z panelu Przelewy24
];
// Sklejanie parametr車w w ci?g JSON
$combinedString = json_encode($params, JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES);
// Hashowanie za pomoc? SHA-384
$hash = hash('sha384', $combinedString);
echo 'Suma kontrolna parametr車w wynosi: ' . $hash;
Weryfikacja transakcji
Nale?y zwr車ci? szczeg車ln? uwag? przy implementacji kodu do wyliczania sumy kontrolnej dla ??dania weryfikacji transakcji i nie tylko. ??danie weryfikacji transakcji w odr車?nieniu do ??dania rejestracji transakcji zawiera jeden nowy parametr, czyli orderId.

Parametr orderId jest parametrem ustalanym przez Przelewy24 i jest to numeryczny identyfikator transakcji (typu integer). Warto?? orderId mo?na przechwyci? z notyfikacji, kt車ra jest wysy?ana na adres urlStatus.

Przyk?ady wyliczania sumy kontrolnej sign dla ??dania weryfikacji transakcji:

PHP

php
$params = [
    'sessionId' => 'unikalne-id-sesji', // Tutaj nale?y umie?ci? unikalne wygenerowane ID sesji
    'orderId' => 999999, // Tutaj nale?y umie?ci? numeryczne ID transakcji odebrany np. z notyfikacji
    'amount' => 1234, // Tutaj nale?y umie?ci? kwot? transakcji w groszach, 1234 oznacza 12,34 PLN
    'currency' => 'PLN', // Tutaj nale?y umie?ci? walut? transakcji
    'crc' => 'crc-z-panelu-p24', // Tutaj nale?y umie?ci? pobrany klucz CRC z panelu Przelewy24
];
// Sklejanie parametr車w w ci?g JSON
$combinedString = json_encode($params, JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES);
// Hashowanie za pomoc? SHA-384
$hash = hash('sha384', $combinedString);
echo 'Suma kontrolna parametr車w wynosi: ' . $hash;
Kalkulator sumy kontrolnej
[Link do kalkulatora]

Dodatkowa funkcjonalno?? API
Test Access
Test po??czenia. REST korzysta ze standardu autoryzacji "basicAuth", gdzie login i has?o to, odpowiednio, ID konta w P24 i klucz API (klucz do raport車w) uzyskany z sekcji ※Moje dane§.

Authorizations: basicAuth

Responses:

200 OK

400 Bad Request

401 Unauthorized

GET /api/v1/testAccess

Response samples
200

json
{
  "data": true,
  "error": "string"
}
Metody p?atno?ci
Metoda zwraca list? dost?pnych metod p?atno?ci.

Authorizations: basicAuth

Path Parameters:

lang required string Enum: "pl" "en"
Kod wybranego j?zyka. Dost?pne: pl , en

Query Parameters:

amount integer
Kwota transakcji. Parametr pozwala zweryfikowa? czy dana metoda p?atno?ci jest dost?pna dla konkretnej kwoty.

currency string Default: "PLN"
Warto?? waluty zgodna z ISO np. PLN

Responses:

200 Lista metod p?atno?ci

403 Not authorized.

404 Payment methods not found

GET /api/v1/payment/methods/{lang}?amount=150&currency=PLN

Response samples
200

json
{
  "data": [{}],
  "agreements": [],
  "responseCode": ""
}
Zwrot transakcji
Zwr車? jedn? lub wiele transakcji.

Authorizations: basicAuth
Request Body schema: application/json
required

Parametr 'refunds' mo?e zawiera? wiele zwrot車w.

Parameter	Typ	Opis
requestId	required string <= 45 characters	Indywidualne ID ??dania
refunds	required Array of objects (RefundRequestArrayDataBasic)	
refundsUuid	required string <= 35 characters	Indywidualne ID dla poprawnego ??dania zwrotu w systemie Merchanta
urlStatus	string	Adres do przekazania danych zwrot車w
Responses:

201 Created. Parametr 'data' zawiera wszystkie zwroty.

400 Invalid input data

401 Not authorized

409 Conflict

500 Unknown error

POST /api/v1/transaction/refund

Request samples
Payload
Content type: application/json

json
{
  "requestId": "string",
  "refunds": [{}],
  "refundsUuid": "string",
  "urlStatus": "string"
}
Response samples
201

json
{
  "data": [{}],
  "responseCode": 0
}
Rejestracja transakcji offline
Ta metoda umo?liwia rejestrowanie p?atno?ci offline. Aby skorzysta? z tej metody, w pierwszej kolejno?ci trzeba zarejestrowa? standardow? transakcj? p?atnicz? z u?yciem metody transaction/register.

Dodatkowo mo?na kontrolowa?, w kt車rym banku zostanie wykonana p?atno??, za pomoc? parametru method.

Authorizations: basicAuth
Request Body schema: application/json
required

Input parameters.

Parameter	Typ	Opis
token	string	
Responses:

200 Successful response

400 Invalid input data

401 Not authorized

409 Conflict

500 Undefined error

POST /api/v1/transaction/registerOffline

Request samples
Payload
Content type: application/json

json
{
  "token": "string"
}
Response samples
200

json
{
  "data": {
    "orderId": 0,
    "sessionId": "string",
    "amount": 0,
    "statement": "string",
    "iban": "string",
    "ibanOwner": "string",
    "ibanOwnerAddress": "string"
  },
  "responseCode": 0
}
Split Payment
Obci??anie p?atno?ci w trybie Split Payment odbywa si? z wykorzystaniem uprzednio zarejestrowanego tokenu w procesie analogicznym do transaction/register. Podczas rejestracji tokenu, nale?y doda? obiekt splitPaymentDetails, charakterystyczny dla tej formy wykonania transakcji.

Authorizations: basicAuth
Request Body schema: application/json
Array

[Szczeg車?owe parametry - patrz dokumentacja pe?na]

Responses:

200 Successful operation

400 Bad request

401 Not authorized

POST /api/v1/transaction/register/splitpayment

Dane zwrotu dla OrderID
Uzyskaj szczeg車?y zwrotu na podstawie ID zam車wienia.

Authorizations: basicAuth

Path Parameters:

orderId required any
Id zam車wienia dla istniej?cego zwrotu

Responses:

200 ??danie zosta?o pomy?lnie przetworzone. Parametr 'data' zawiera dane zwrotu.

401 Not authorized

404 Refund with given Order Id not found

500 Undefined error

GET /api/v1/refund/by/orderId/{orderId}

Dane o transakcji poprzez sessionID
Metoda zwraca informacje o transakcji na podstawie pola ※sessionId§.

Authorizations: basicAuth

Path Parameters:

sessionId required any
Unikalny identyfikator transakcji z systemu sprzedawcy

Responses:

200 OK

400 Invalid input data

401 Incorrect authentication

404 Transaction not exist

GET /api/v1/transaction/by/sessionId/{sessionId}

Notyfikacja o zwrocie
Wynik zwrotu
Notyfikacja o zwrocie wysy?ana jest w spos車b asynchroniczny na adres URL podany w ??daniu wykonania zwrotu transaction/refund w parametrze urlStatus. Je?li nie zostanie przekazana warto?? w urlStatus, to notyfikacja zostanie przes?ana na domy?lny adres ustawiony w panelu P24 (o ile taki adres zosta? skonfigurowany).

Aby skonfigurowa? domy?lny adres URL w panelu, prosz? o kontakt z Biurem Obs?ugi Klienta poprzez formularz kontaktowy

json
{
  "orderId": integer <int64>,
  "sessionId": string,
  "merchantId": integer,
  "requestId": string,
  "refundsUuid": string,
  "amount": integer,
  "currency": string,
  "timestamp": integer,
  "status": integer Enum: 0 1,
  "sign": string
}
Suma kontrolna parametr車w:
{"orderId":int,"sessionId":"str","refundsUuid":"str","merchantId":int,"amount":int,"currency":"str","status":int,"crc":"str"}

liczona z u?yciem sha384

WA?NE!:
przy wykorzystaniu funkcji json_encode nale?y doda? nast?puj?ce atrybuty
"JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES"

Przypadki u?ycia metod p?atno?ci
PayPo
Metoda p?atno?ci PayPo nie jest domy?lnie uruchomion? form? p?atno?ci i jest dost?pna po kontakcie z naszym dzia?em Merchant Success poprzez formularz kontaktowy.

Po uruchomieniu us?ugi na Pa里stwa koncie, aby prawid?owo zarejestrowa? transakcj? nale?y w ??daniu rejestracji transakcji przekaza? dodatkowe parametry, kt車re domy?lnie s? opcjonalne: client, city, zip, address.

Kwota przesy?ana w ??daniu obecnie minimalnie wynosi 5z?, jej maksymalna warto?? 5 000z?.

P?atno?ci PayPo wyp?acane s? po otrzymaniu ?rodk車w od operatora. domy?lnie merchant otrzyma ?rodki do 5 dni roboczych.

PayPal
Aby metoda PayPal dzia?a?a poprawnie na Pa里stwa stronie musi zosta? ona uruchomiona dla konta po stronie Przelewy24. W celu uruchomienia metody PayPal prosimy o kontakt wykorzystuj?c adres mailowy przypisany w panelu Przelewy24 do konta poprzez formularz kontaktowy.

Aby poprawnie wykona? transakcj? dla wskazanej metody konieczne jest przes?anie pe?nego obiektu cart z wymaganymi parametrami wys?anymi w ??daniu rejestracji transakcji.

Przypadki u?ycia p?atno?ci kart?
Wprowadzenie
Do realizacji standardowych transakcji kartowych procesowanych poprzez paymentwall P24 wystarczy wykona? standardow? integracj? (payment service) i nie ma konieczno?ci implementowania poni?szych rozwi?za里. Opisane poni?ej rozwi?zania s? dodatkowymi funkcjonalno?ciami kartowymi.

Funkcjonalno?ci kartowe nie s? domy?lnie w??czone, a ich zakres oraz spos車b i mo?liwo?ci testowania s? zale?ne od Operatora. Skontaktuj si? z Dzia?em Obs?ugi Technicznej poprzez formularz kontaktowy, aby uzyska? wi?cej informacji.

Jak uruchomi? p?atno?? kart? wewn?trz sklepu?
Aby ograniczy? liczb? krok車w procesu p?atno?ci, mo?na umie?ci? formularz kartowy bezpo?rednio na stronie sklepu. Implementacja rozwi?zania nie wp?ywa na przetwarzanie danych kartowych - dane s? nadal przetwarzane wy??cznie przez Przelewy24, co pozwala zachowa? wszystkie wymagania zwi?zane z bezpiecze里stwem kart kredytowych i zachowuje zgodno?? ze standardem PCI DSS.

Card Payment
W celu wykonania p?atno?ci za pomoc? kart p?atniczych, konieczne jest zarejestrowanie transakcji w systemie Przelewy24 z u?ycie metody transaction/register, a nast?pnie przekazanie otrzymanego tokenu do wywo?ania w skrypcie JS Przelewy24. Dane wprowadzone w formularzu zostan? przekazane bespo?rednio do systemu Przelewy24, w wyniku czego aktywowany zostanie skrypt (wskazany w konfiguracji) po stronie sklepu.

Do realizacji p?atno?ci za pomoc? kart po stronie sklepu wymagana jest odpowiednia umowa 每 prosz? o kontakt z Dzia?em Handlowym Przelewy24 poprzez formularz kontaktowy.

Proces p?atno?ci kart? wewn?trz sklepu
Rejestracja transakcji poprzez metod? transaction/register i pobranie Tokenu transakcji

Przygotowanie elementu DIV w tre?ci strony, gdzie ma zosta? zamieszczony formularz p?atno?ci kart?

Przygotowanie skryptu Javascript, kt車ry zostanie wywo?any po zako里czeniu transakcji

Przygotowanie pola do zamieszczenia formularza rejestracji karty

Do przygotowania miejsca na stronie, gdzie ma zosta? wy?wietlony formularz rejestracji karty mo?na wykorzysta? tag DIV. Atrybut ID tego elementu nale?y ustawi? na warto?? "P24FormContainer". Kod pola wygl?da nast?puj?co:

html
<div
  id="P24FormContainer"
  data-sign="{P24SIGN}"
  data-successCallback="{FinishPaymentFunction}"
  data-failureCallback="{PaymentErrorFunction}"
  data-dictionary='{DICTIONARY JSON}' >
</div>
Gdzie:

{P24SIGN} 每 suma kontrolna taka sama, jak u?yta w ??daniu transaction/register

{FinishpaymentFunction} 每 nazwa funkcji wywo?anej w przypadku poprawnej transakcji z jednym parametrem wej?ciowym - ID transakcji (integer) nadanym przez Przelewy24

{PaymentErrorFunction} 每 nazwa funkcji wywo?anej w przypadku b??dnej transakcji, funkcja przyjmuje jeden parametr - kod b??du (integer)

{DICTIONARY JSON} 每 s?ownik termin車w u?ytych w formularzu p?atno?ci, jak poni?ej:

json
{
  "cardHolderLabel": "string",
  "cardNumberLabel": 0,
  "cvvLabel": 0,
  "expDateLabel": "string",
  "payButtonCaption": "string",
  "threeDSAuthMessage": "string"
}
Skrypt generuj?cy formularz w wewn?trz DIV#P24FormContainer nale?y za??czy? do strony:

GET https://secure.przelewy24.pl/inchtml/ajaxPayment/ajax.js?token={TOKEN}

Gdzie, w miejsce {TOKEN} nale?y wstawi? Token otrzymany w wyniku dzia?ania metody transaction/register. Opcjonalnie mo?na wykorzysta? style CSS dla formularza rejestracji karty, lub alternatywnie zastosowa? w?asne.
Adres URL domy?lnych styli:
https://secure.przelewy24.pl/inchtml/ajaxPayment/ajax.css

W przypadku transakcji wymagaj?cej dodatkowej autoryzacji (3DSecure) po wype?nieniu formularza na stronie pojawi si? link prowadz?cy do nowego okna z formularzem autoryzacji (np. wpisanie SMSa wys?anego z banku). Po poprawnej autoryzacji okno zostanie zamkni?te i nast?pi wywo?anie funkcji Javascript dla poprawnej transakcji.

Adres powrotny przekazany w parametrze "urlReturn" powinien prowadzi? do skryptu zamieszczonego na tej samej domenie co skrypt ??dania. Powinien on uruchamia? nast?puj?c? funkcj?:

javascript
window.setTimeout(function(){
  opener.P24_Transaction.threeDSReturn(window);
  window.close();
},1000);
Jak zarejestrowa? kart? lub wykona? p?atno???
Rejestracj? karty nale?y wykona? jak przy zwyk?ej p?atno?ci. P?atno?? kart? mo?na wykona? na 3 sposoby:

paymentwall p24

bezpo?rednie API (card / pay) - je?eli dane kartowe s? procesowane przez zasoby Merchanta, rozwi?zanie wymaga PCI DSS

formularz wewn?trz sklepu

Nast?pnie poprzez card/info lub dodatkow? notyfikacj?, mo?na pobra? informacj? o karcie, w tym numer referencyjny, niezb?dny do p車?niejszych obci??e里.

Nast?pnie, w zale?no?ci od tego, czy chcemy przyj?? p?atno??, wykonujemy transaction/verify lub transaction/verify a nast?pnie transaction/refund , je?eli by?a to tylko p?atno?? do pr車bkowania np. na 1 PLN.

Jak pozyska? informacje o karcie?
S? dwa sposoby na pozyskanie informacji na temat karty:

Wywo?a? metod? card/info

Obs?u?y? dodatkow? notyfikacj? kartow?

Dla dowolnych transakcji kartowych notyfikacja mo?e zosta? u?yta:

w procesie card/chargeWith3ds, card/charge, card/pay opartym o REST, aby m車c natychmiastowo wy?wietli? komunikat klientowi o udanej/nieudanej transakcji

w procesie RISK po stronie Partnera, dla sprawdzenia karty klienta i uchronienia si? przez fraudami

w przypadku zapisywania karty do procesu 1-click, nie ma konieczno?ci dodatkowego wykonywania zapytania o numer referencyjny karty

OneClick
Jak wykona? p?atno?? 1-click z 3ds?
Do tego celu s?u?y dedykowana metoda card/chargeWith3ds

Numer referencyjny karty musi zosta? przekazany podczas rejestracji transakcji w parametrze methodRefId.

Metoda zwr車ci link do przekierowania klienta i obs?u?enia 3ds.

Jak obs?u?y? p?atno?ci bez udzia?u klienta (p?atno?ci rekurencyjne)?
Metoda card/charge umo?liwia rekurencyjne obci??anie karty na podstawie przekazanego numeru referencyjnego.

Numer referencyjny karty musi zosta? przekazany podczas rejestracji transakcji w parametrze methodRefId.

Warto?? parametru refId otrzymana z transakcji op?aconych przez metody walletowe (np. Google Pay, Apple Pay, VISA Mobile) nie mo?e by? zastosowana w p?atno?ciach rekurencyjnych. Pr車ba op?acenia takiej transakcji b?dzie skutkowa?a b??dem oraz ostatecznie brakiem wp?aty.

Do zainicjowania procesu nie jest wymagany udzia? klienta.

Jak obs?u?y? 3ds (w tym 3ds 2.X)?
Zar車wno 3ds jak i 3ds 2.X obs?ugiwane s? w ten sam spos車b. Dla rozwi?zania opartego o paymentwall p24 czy formatk? wewn?trz sklepu, 3ds obs?ugiwany jest automatycznie.

W przypadku skorzystania z metody card/pay lub card/chargeWith3ds przekierowanie nale?y wykona? samemu, na adres URL uzyskany z odpowiedzi.

Pami?taj, ?e po wykonaniu 3ds, klient zawsze zostanie przekierowany na stron? urlReturn. Merchant musi przeprocesowac powr車t klienta ze strony banku do sklepu. Zwr車cony link jest aktywny przez 15 minut.

Jak zarejestrowa? kart? lub zap?aci? w aplikacji mobilnej?
Rejestracj? karty lub p?atno?? w aplikacji mobilnej mo?na dokona? na dwa sugerowane sposoby. Mo?na otworzy? paymentwall p24 w WebView, najlepiej z wymuszon? form? p?atno?ci dla danych kart. Innym sposobem jest wykorzystanie metody card/pay. Nale?y pami?ta?, aby metoda card/pay by?a wywo?ana bezpo?rednio z aplikacji do P24 bez udzia?u serwer車w Merchanta. W innym przypadku niezb?dne b?dzie posiadanie certyfikatu PCI DSS.

Jak przetestowa? karty w ?rodowisku Sandbox?
Aby przetestowa? p?atno?? kart? w ?rodowisku testowym nale?y skorzysta? z numer車w kart z dokumentacji Saferpay oraz dowolnej daty jako expiry date z przysz?o?ci i losowego 3-cyfrowego CVV.

Do testowania Google Pay zalecamy do??czenie do grupy developerskiej Google Google Pay API Test Cards Allowlist - Google Groups .

Po do??czeniu do grupy, do konta Google przypisanych zostanie kilka numer車w kart testowych (od Google) wybieralnych po wci?ni?ciu przycisku do p?atno?ci Google Pay.

Nale?y pami?ta?, ?e us?ugi kartowe nie s? domy?lnie uruchomione dla ?rodowiska produkcyjnego jak i ?rodowiska Sandbox.

Czy Merchant b?dzie przetwarza? pe?ne dane karty?
Do przetwarzania pe?nych danych kartowych niezb?dny jest certyfikat PCI DSS. Rozwi?zania P24 wspieraj? operacje wykonywane w ten spos車b.

Jednak?e istnieje szereg rozwi?za里 zapewniaj?cych merchantowi pe?n? funkcjonalno?? kartow?, w kt車rych nie b?dzie pos?ugiwa? si? pe?nymi danymi kartowymi i nie ma potrzeby posiadania PCI DSS. W takich sytuacjach Merchant pozyskuje jedynie numer referencyjny karty s?u??cy, np. do p車?niejszych obci??e里 typu 1-click czy recurring, oraz zestaw niewra?liwych danych, tj. data wa?no?ci czy BIN.

Karty API
Card info
Metoda zwraca informacj? na temat danej karty p?atniczej na podstawie poprzedniej p?atno?ci. W??czaj?c numer referencyjny do obci??enia kart bez autoryzacji CVV.

Authorizations: basicAuth

Path Parameters:

orderId required integer <int64>
Unikalne ID zam車wienia.

Responses:

200 Success

400 Wrong input data

403 Not authorized

404 Transaction not exists

GET /api/v1/card/info/{orderId}

Response samples
200

json
{
  "data": {
    "refId": "string",
    "bin": 0,
    "mask": "string",
    "cardType": "string",
    "cardDate": "string",
    "hash": "string"
  },
  "responseCode": 0
}
Charge card with 3DS
Metoda umo?liwia obci??enie karty na podstawie numeru referencyjnego.

Authorizations: basicAuth
Request Body schema: application/json

Parameter	Typ	Opis
token	string	Token zarejestrowany metod? transaction/register. Numer referencyjny karty musi zosta? przekazany w trakcie rejestracji w parametrze methodRefId
Responses:

200 The charge card command has been accepted - notification will be send on success.

201 The card payment requires 3DS redirection

400 Invalid input data

401 Not authorized

POST /api/v1/card/chargeWith3ds

Request samples
Payload
Content type: application/json

json
{
  "token": "string"
}
Response samples
201

json
{
  "data": {
    "orderId": 0,
    "redirectUrl": "string"
  },
  "responseCode": 0
}
Charge card
Metoda umo?liwia obci??enie karty na podstawie numeru referencyjnego.

Authorizations: basicAuth
Request Body schema: application/json

Parameter	Typ	Opis
token	string	Token zarejestrowany metod? transaction/register. Numer referencyjny karty musi zosta? przekazany w trakcie rejestracji w parametrze methodRefId
Responses:

200 The charge card command has been accepted - notification will be send on success.

400 Invalid input data

401 Not authorized

POST /api/v1/card/charge

Card Payment
Metoda s?u?y do obci??enia karty klienta. Metoda przesy?a dane kartowe bezpo?rednio.

Authorizations: basicAuth
Request Body schema: application/json

Parameter	Typ	Opis
transactionToken	required string	Token pozyskany w procesie rejestracji
cardNumber	required string <= 16 characters	Numer karty
cardDate	required string	Data wa?no?ci w formacie MMYYYY
cvv	required string	Card CVV
clientName	required string	Imi? i nazwisko posiadacza karty
Responses:

200 The card payment has been succesed.

201 The card payment requires 3DS redirection.

400 Invalid input data

401 Not authorized

409 Conflict

POST /api/v1/card/pay

Dodatkowa notyfikacja kartowa
Notyfikacja jest wysy?ana na adres z parametru "urlCardPaymentNotification", kt車ry nale?y doda? do metody transaction/register lub na sta?y zapisany adres w konfiguracji konta P24. Nadrz?dna jest warto?? z tokenu, je?eli zosta?a przes?ana.

Parametry dla pozytywnej autoryzacji
json
{
  "amount": integer,
  "3ds": boolean,
  "method": integer,
  "refId": string,
  "orderId": integer <int64>,
  "sessionId": string,
  "bin": integer,
  "maskedCCNumber": string,
  "ccExp": string,
  "hash": string,
  "cardCountry": string,
  "risk": integer,
  "liabilityshift": boolean,
  "sign": string
}
Suma kontrolna parametr車w:
{"amount":int,"3ds":boolean,"method":int,"refId":"str","orderId":int,"sessionId":"str","bin":int,"maskedCCNumber":"str","ccExp":"str","hash":"str","cardCountry":"str","risk":int,"liabilityshift":boolean,"crc":"str"}

liczona z u?yciem sha384

Wa?ne!:
w przypadku wykorzystania funkcji json_encode, powinny zosta? dodane nast?puj?ce atrybuty
"JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES"

Parametry dla negatywnej autoryzacji
json
{
  "amount": integer,
  "3ds": boolean,
  "method": integer,
  "orderId": integer <int64>,
  "sessionId": string,
  "errorCode": string,
  "errorMessage": string,
  "sign": string
}
Przed wyliczeniem signa, nale?y przekszta?ci? warto?? parametru w taki spos車b, aby znaki alfanumeryczne zosta?y zamienione na diaktryczne.

Suma kontrolna parametr車w:
{"amount":int,"3ds":boolean,"method":int,"orderId":int,"sessionId":"str","errorCode":"str","errorMessage":"str","crc":"str"}

Liczona z u?yciem SHA384

Wa?ne!:
w przypadku wykorzystania funkcji json_encode, powinny zosta? dodane nast?puj?ce atrybuty
"JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES"

Przypadki u?ycia BLIK
Wprowadzenie
Opr車cz standardowej p?atno?ci bazuj?cej na przekierowaniu do paymanetwall P24, na stronie Merchanta mo?na r車wnie? umie?ci? p?atno?ci BLIK.

Jak umie?ci? p?atno?ci BLIK na stronie Merchanta? (BLIK level 0)
Po zarejestrowaniu transakcji z u?yciem metody transaction/register, wywo?ujemy metod? paymentMethod/blik/chargeByCode, z obowi?zkowym wykorzystaniem parametr車w ※token§ (zwracany w odpowiedzi z rejestracji transakcji) i ※blikCode§ (wpisany przez u?ytkownika).

Co to jest p?atno?? BLIK 1-click?
Jak sama nazwa sugeruje, p?atno?? pozwala na wykonanie zakupu jednym klikni?ciem. Nie wymaga to przepisania kodu BLIK, jedynie proste potwierdzenie w aplikacji mobilnej jednym klikni?ciem.

Jak uzyska? informacje o aliasie?
S? dwa sposoby na uzyskanie informacji o aliasie:

Pozyskanie informacji poprzez obs?u?enie dodatkowej notyfikacji. (zalecane)

W celu otrzymania aliasu dla kolejnych p?atno?ci, wywo?aj metod? getAliasesByEmail.
Metoda zwraca list? alias車w (razem z typem i statusem) utworzonych dla danego adresu w zakresie danego merchanta.
Dla alias車w zarejestrowanych z parametrami values wywo?aj metod? getAliasesByEmail{email}/custom

W zwi?zku z asynchronicznym charakterem przetwarzania statusu transakcji, czas zwr車cenia aktualnego statusu alias車w przez metod? getAliasesByEmail mo?e wynie?? do 60 sekund po poprawnie dokonanej transakcji.

Jak zarejestrowa? alias do wykorzystania w p?atno?ciach 1-click?
Transakcj? mo?na przetworzy? metod? paymentMethod/blik/chargeByCode. Us?uga pozwala na przypisanie indywidualnych warto?ci alias value i alias label w parametrach wej?ciowych. Je?li w procesie rejestracji transakcji parametr ※referenceRegister§ = true, to ??danie rejestracji aliasu zostanie przekazane do systemu BLIK, a klient otrzyma zaproszenie, wygenerowane przez aplikacje bankow?, do p?atno?ci bez kodu T6 w sklepie merchanta.

Po wywo?aniu tej metody, powstanie obci??enie na kwot? przekazan? w rejestracji transakcji i zostanie zarejestrowany alias w systeme P24/BLIK.

Utworzony alias mo?e by? wykorzystywany do przetwarzania p?atno?ci typu OneClick.

Po zarejestrowaniu aliasu, klient nie b?dzie proszony o wprowadzanie kodu T6 podczas nast?pnych p?atno?ci.

W celu zarejestrowania innej aplikacji mobilnej w systemie BLIK, nale?y skorzysta? z metody paymentMethod/blik/chargeByCode wraz z 6-cyfrowym kodem BLIK, wygenerowanym przez aplikacj?. W systemie P24 transakcja musi by? zarejestrowana na ten sam adres e-mail klienta.

Alias tworzony jest na podstawie adresu e-mail przekazanego w procesie rejestracji. Oznacza to, ?e o ile nie zosta?y wykorzystane parametry ※aliasValue§ i ※aliasLabel§, to na ten sam adres e-mail mo?e by? zarejestrowany tylko jeden alias danego typu.

Aby zarejestrowa? wi?cej ni? jeden alias dla danego adresu e-mail, nale?y skorzysta? z alias value i alias label. W ten spos車b merchant zapewni, ?e aliasy s? unikalne. Lista alias車w merchanta mo?e by? pozyskana za pomoc? metody getAliasesByEmail. Mo?liwe jest rejestrowanie tego samego aliasu zdefiniowanego przez merchanta dla kilku r車?nych adres車w e-mail. W tym przypadku metoda getAliasesByEmail zwraca ten sam alias dla ka?dego z adres車w e-mail.

Jak wykona? p?atno?? 1-click (tylko 1-click)?
ChargeByAlias jest metod? p?atno?ci typu one click. Pozwala na obci??enie klienta korzystaj?c z uprzednio pozyskanego aliasu. Pozyskany alias musi by? przekazany w parametrze methodRefId w trakcie rejestracji transakcji(transaction/register). Ustaw typ type=alias.

Jak obs?u?y? dwie zarejestrowane aplikacje na jeden alias (tylko 1-click)?
MetodaChargeByAlias s?u?y do wykonywania obci??e里 ?rodk車w klienta za pomoc? wcze?niej pobranego aliasu wraz z podaniem ※alternativeKey§ klucza identyfikuj?cego aplikacj? mobiln? klienta Pojawia si? dodatkowy parametr "alternativeKey".

Metod? nale?y wykona? tylko w przypadku otrzymania odpowiedzi z metody chargeByAlias z typem ?alias§ z kodem b??du 51 i httpcode 409 (Wybrany alias do identyfikacji jest niejednoznaczny!) i z list? alternatywnych kluczy identyfikuj?cych aplikacje mobilne klienta 每 "AliasAlternative".

Klient powinien wybra? aplikacj? mobiln?, z kt車rej zostanie dokonane obci??enie. Alias powinien zostac wybrany z listy the "AliasAlternative".

Metoda z typem "alternativeKey" § b?dzie wykorzystywana tylko w przypadku, gdy klient posiada wi?cej ni? jedn? aplikacj? mobiln? podpi?t? pod ten sam alias typu UID.

Podczas obs?ugi b??du 51 sprzedawca nie powinien zapisywa? alternatywnych kluczy i labeli. Dane te ulegaj? zmianie na poziomie bank / u?ytkownik aplikacji bankowej.

Jak poradzi? sobie z przeterminowanym aliasem (tylko 1-click)?
Gdy transakcja zostanie odrzucona z powodu b??du 68 (przedawnienia aliasu klienta), nale?y zarejestrowa? ponownie transakcj? wraz z nowym aliasem - zgodnie z informacjami zawartymi w sekcji o rejestracji aliasu.

Jak mog? przetestowa? BLIK (whitelabel) w Sandbox?
Aby przetestowa? p?atno?? BLIK Whitelabel za pomoc? kodu T6 w ?rodowisku Sandbox nale?y u?y? 6 cyfrowego kodu BLIK w formacie 777XXX dla udanej transakcji gdzie X to dowolna cyfra z zakresu od 0-9. Dla symulacji nieudanej transakcji nale?y u?y? dowolnych 6 cyfr. Zalecamy r車wnie? nie korzystanie z tego samego kodu BLIK w kr車tkich odst?pach czasu celem zapewnienia poprawnych odpowiedzi.

Tytu? przelewu widoczny w aplikacji klienta?
Domy?lnie, w aplikacji bankowej klienta, jako g?車wny tytu? widoczny jest numer transakcji P24. Dodatkowe linie pokazuj? r車wniez informacje przes?ane w parametrze description w ??daniu transaction/register, jako pomocnicze dane.

Klient mo?e r車wnie? zobaczy? spersonalizowan? warto?? przes?an? przez merchanta, zamiast numeru transkacji P24 w g?車wnym tytule. W tym przypadku ??danie transaction/register powinno zawiera? parametr transferLabel, kt車ry nadpisze g?車wny tytu? przelewu.

BLIK API
BLIK charge by code
Umo?liwia obci??enie transakcji za pomoc? kodu T6. Zwraca unikalny identyfikator transakcji.

Authorizations: basicAuth
Request Body schema: application/json

Parameter	Typ	Opis
token	required string	Token uzyskany podczas rejestracji transakcji ??daniem transaction/register.
WA?NE!:
Aby poprawnie obci??y? p?atnika metod? blikChargeByCode nale?y w ??daniu transaction/register przes?a? w obiekcie additional obiekt PSU.
blikCode	required string	6-cyfrowy, jednorazowy kod BLIK, wygenerowany w aplikacji
aliasValue	string	Unikalny alias u?ytkownika, kt車ry mo?e by? u?yty do obci??enia ?rodk車w podczas kolejnych transakcji.
WA?NE!: Parametr wymagany je?li w ??daniu przes?ano obiekt recurring.
aliasLabel	string [ 5 .. 35 ] characters	Etykieta aliasu wy?wietlana w aplikacji.
WA?NE!: Parametr wymagany je?li w ??daniu przes?ano obiekt recurring.
recurring	object (RecurringParamsIn)	Obiekt zawieraj?cy infomacje dotycz?ce p?atno?ci cyklicznej BLIK.
Funkcjonalno?? p?atno?ci cyklicznej nie jest domy?lnie w??czona. Skontaktuj si? z Dzia?em Obs?ugi Technicznej poprzez formularz kontaktowy, w celu uruchomienia us?ugi.
Responses:

201 Created

400 Invalid input data

401 not authorized

500 Undefined error

POST /api/v1/paymentMethod/blik/chargeByCode

BLIK charge by Alias
ChargeByAlias to metoda p?atno?ci typu one-click. Pozwala na obci??enie ?rodk車w klienta, korzystaj?c z uprzednio pozyskanego aliasu (getAliasesByEmail). Pozyskany alias musi zostac przekazany w parametrze methodRefId podczas rejestracji transakcji

Authorizations: basicAuth
Request Body schema: application/json

Parameter	Typ	Opis
token	required string	Token uzyskany podczas rejestracji transakcji ??daniem transaction/register.
WA?NE!:
Aby poprawnie obci??y? p?atnika metod? blikChargeByAlias nale?y w ??daniu transaction/register przes?a? w obiekcie additional obiekt PSU.
type	required string	Ustaw warto?? ?alias§
aliasValue	string	Uwaga! Wys?anie parametru spowoduje nadpisanie istniej?cego aliasu.
Unikalny alias u?ytkownika, kt車ry mo?e by? u?yty do obci??enia ?rodk車w podczas kolejnych transakcji.
WA?NE!: Parametr wymagany je?li w ??daniu przes?ano obiekt recurring.
aliasLabel	string	Uwaga! Wys?anie parametru spowoduje nadpisanie istniej?cej etykiety.
Etykieta aliasu wy?wietlana w aplikacji.
WA?NE!: Parametr wymagany je?li w ??daniu przes?ano obiekt recurring.
recurring	object (RecurringParamsIn)	Obiekt zawieraj?cy infomacje dotycz?ce p?atno?ci cyklicznej BLIK.
Funkcjonalno?? p?atno?ci cyklicznej nie jest domy?lnie w??czona. Skontaktuj si? z Dzia?em Obs?ugi Technicznej poprzez formularz kontaktowy, w celu uruchomienia us?ugi.
Responses:

201 Created

400 Bad request

401 Not authorized

409 AlternativeKeys

500 Undefined error

POST /api/v1/paymentMethod/blik/chargeByAlias

Getting Aliases ByEmail
Aby uzyska? alias do kolejnych p?atno?ci, skorzystaj z metody "getAliasesByEmail"

Metoda zwraca list? alias車w (wraz z typem i statusem) utworzonych dla danego adresu e-mail w zakresie danego merchanta.

Authorizations: basicAuth

Path Parameters:

email required any
Dla alias車w zarejestrowanych poprzez e-mail

Responses:

200 OK

400 Bad Request

401 Unauthorized

404 Alias not found

GET /api/v1/paymentMethod/blik/getAliasesByEmail/{email}

Getting Aliases ByEmail (Custom)
Aby uzyska? alias do kolejnych p?atno?ci, skorzystaj z metody "getAliasesByEmail"

Metoda zwraca list? alias車w (wraz z typem i statusem) utworzonych dla danego adresu e-mail w zakresie danego merchanta.

Authorizations: basicAuth

Path Parameters:

email required any
Dla alias車w zarejestrowanych z warto?ciami "aliasValue" i "aliasLabel"

Responses:

200 OK

400 Bad Request

401 Unauthorized

404 Alias not found

GET /api/v1/paymentMethod/blik/getAliasesByEmail/{email}/custom

Dodatkowa notyfikacja BLIK
Dla dowolnych transakcji realizowanych przez BLIK, zosta?a wprowadzona opcjonalna dodatkowa notyfikacja o statusie p?atno?ci.

Notyfikacja jest wysy?ana na adres z parametru "urlCardPaymentNotification", kt車ry nale?y doda? do metody transaction/register lub na sta?y zapisany adres w konfiguracji konta P24. Nadrz?dna jest warto?? z tokenu, je?eli zosta?a przes?ana.

json
{
  "data": {
    "orderId": 0,
    "sessionId": "string",
    "method": 0,
    "result": {},
    "sign": "string"
  }
}
Notyfikacja uaktualnienia aliasu
Dodatkowa notyfikacja mo?e by? wys?ana na okre?lony adres URL, je?li status zosta? utworzony lub zmieniony. Adres jest konfigurowany poprzez us?ug? P24. Notyfikacja mo?e by? wykorzystana jako alternatywa do metody getAliasesByEmail.

json
{
  "email": "string",
  "value": "string",
  "type": "string",
  "status": "string"
}
Raport API
Historia transakcji
Aby w??czy? tak? funkcjonalno?? nale?y skontaktowa? si? z opiekunem handlowym poprzez formularz kontaktowy

Metoda zwraca informacje na temat: paczek (batch), transakcji i zwrot車w, w zadanym okresie czasu.

Stronicowanie
Aby wywo?a? nastepna stron?, nale?y wys?a? ??danie report/history z parametrem token, np.:
GET https://secure.przelewy24.pl/api/v1/report/history/{token}

Authorizations: basicAuth

Path Parameters:

dateFrom required string
Data w formacie YYYYMMDD

dateTo required string
Data w formacie YYYYMMDD. Maksymalny okres czasu to 31 dni

type string
Typ obiekt車w do za?adowania. Omi里 ten parametr, je?li chcesz pobra? wszystkie dane. Akceptowane warto?ci: batch, transaction, refund.

Responses:

200 OK

400 Bad request

401 Not authorized

500 Undefined error

GET /api/v1/report/history

Informacje o paczce (batch)
Aby w??czy? tak? funkcjonalno?? nale?y skontaktowa? si? z opiekunem handlowym poprzez formularz kontaktowy

Metoda zwraca wszystkie transakcje i zwroty op?acone w zadanej paczce (batch).

Stronicowanie
Aby wywo?a? nast?pn? strone nale?y wywo?a? ??danie report/batch/details z parametrem token np.:
GET https://secure.przelewy24.pl/api/v1/report/batch/details/{token}

Authorizations: basicAuth

Path Parameters:

batch required integer
Unikalne ID paczki

token string
Token potrzebny do wywo?ania nast?pnej strony

Responses:

200 OK

400 Bad request

401 Not authorized

500 Undefined error

GET /api/v1/report/batch/details

Opis Google Pay
GPay
Google Pay to szybki i prosty spos車b p?atno?ci od Google. Dane karty s? przechowywane bezpiecznie na serwerach firmy Google i pozwalaj? u?ytkownikowi wykona? proces p?atno?ci bez potrzeby manualnego wype?niania formularza kartowego lub kontaktowego.

Google Pay to produkt pozwalaj?cy na uzyskanie zaszyfrowanych danych karty p?atniczej klienta umo?liwiaj?cych obci??enie. Aby wykona? p?atno?? przez Google Pay klient wcze?niej musi zapisa? kart? p?atnicz? pod swoim kontem Google, u?ywaj?c jakiejkolwiek platformy Google (np. kupuj?c aplikacj? w Google Play, p?ac?c za przestrze里 dyskow? na Google Drive, przypisuj?c kart? do swojego konta AdWords) lub bezpo?rednio na stronie https://pay.google.com

Us?uga wymaga wcze?niejszego podpisania umowy z operatorem kartowym. Aby uruchomi? funkcjonalno??, prosz? o kontakt Biurem Obs?ugi Klienta poprzez formularz kontaktowy.

Schemat komunikacji Google Pay
Po klikni?ciu w przycisk "Zap?a? z Google Pay" klientowi pojawia si? na ekranie formatka Google Pay, na kt車rej potwierdza swoje konto Google i kart?, kt車r? ma zamiar zap?aci? (mo?e r車wnie? na tym etapie zmieni? kart? na inn? wcze?niej zapisan?, lub doda? now?). Skrypt w tle przekazuje zakodowane dane karty przez funkcj? postMessage, kt車r? sklep musi przej??, a nast?pnie zakodowa? przez funkcj? base64 ponownie i przes?a? w parametrze methodRefId wraz z danymi transakcji.

Sklep na swojej stronie musi wywo?a? skrypt udost?pniony przez Google. Szczeg車?y dost?pne pod adresem:

https://developers.google.com/pay/api/web/guides/tutorial

Aby doda? Przelewy24 nale?y w wywo?aniu skryptu zmodyfikowa? dane Procesora:

javascript
var tokenizationSpecification = {
  tokenizationType: 'PAYMENT_GATEWAY',
  parameters: {
    gateway: 'przelewy24',
    gatewayMerchantId: '[MerchantID from P24]'
  }
}
Rejestracja transakcji
Rejestracja transakcji przez Google Pay polega na dodaniu dodatkowego parametru "methodRefId" z danymi otrzymanymi z Google przez postMessage (paymentMethodToken.token) i zakodowaniu ich funkcj? base64.

POST https://secure.przelewy24.pl/api/v1/transaction/register

Dodatkowy parametr POST wywo?ania

Field name	Type	Required	Description
methodRefId	STRING	Y	Token otrzymany z Google Pay zakodowany funkcj? base64
Przed wys?aniem ??dania transakcji nale?y zapisa? jej dane do lokalnej bazy danych sprzedawcy. W szczeg車lno?ci nale?y zachowa? informacje o identyfikatorze sesji i kwocie transakcji.

Implementacja dla systemu Android
Aby zaimplementowa? Google Pay na urz?dzeniach z systemem Android zapoznaj si? z dokumentacja Google Pay API: https://developers.google.com/pay/api/android

Informacje dodatkowe
Dla usp車jnienia stylistyki stosowanej na stronie www oraz w aplikacji mobilnej udost?pniony zosta? zestaw dedykowanych wskaz車wek: https://developers.google.com/pay/api/web/guides/brand-guidelines

Zalecenia do tworzenia aplikacji mobilnych dost?pne sa pod adresem: https://developers.google.com/pay/api/android/guides/tutorial

Google Pay API
Przekazanie tokenu
Po otrzymaniu tokenu z Przelewy24 nale?y na stronie wywo?a? skrypt w Javascript.

Po wykonaniu transakcji wywo?ywana jest odpowiednia odpowied? zwrotna (callback). W przypadku transakcji wymagaj?cej dodatkowej autoryzacji (3DSecure) klient zostaje przekierowany, a nast?pnie wraca na adres podany przy rejestracji transakcji w parametrze "urlReturn".

Path Parameters:

TOKEN any
Token uzyskany z Przelewy24.

GET /bundle/payWithGoogle/{TOKEN}

Request samples
HTML

html
<head>
  <meta charset="UTF-8">
  <meta name="viewport"
        content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">
  <meta http-equiv="X-UA-Compatible" content="ie=edge">
  <title>PayWithGoogle</title>
  <script src="https://secure.przelewy24.pl/bundle/payWithGoogle/{{TOKEN}}"></script>
</head>
<body>
<script>
  document.addEventListener("DOMContentLoaded", function () {
      Przelewy24PayWithGoogle.config({
          errorCallback: function () {
              alert("error");
          },
          exceptionCallback: function () {
              alert("exception");
          },
          requestFailedCallback: function () {
              alert('requestFailed');
          },
          completePaymentCallback: function () {
              alert("success");
          }
      });
      Przelewy24PayWithGoogle.charge();
  });
</script>
</body>
Apple Pay
ApplePay
Wprowadzenie
Dzi?ki Apple Pay nie musisz zak?ada? ?adnych kont ani wype?nia? d?ugich formularzy podczas zakup車w na stronach internetowych otwartych w Safari na iPhonie, iPadzie lub Macu. A poniewa? czytnik Touch ID dost?pny jest te? w MacBooku Air i MacBooku Pro, finalizacja transakcji wymaga tylko przy?o?enia palca i jest szybsza, ?atwiejsza i bezpieczniejsza ni? dot?d.

Opis
Podczas zakup車w Apple Pay wykorzystuje numer przypisany konkretnemu urz?dzeniu i unikalny kod transakcji. Dzi?ki temu numer Twojej karty nigdy nie jest przechowywany ani na Twoim urz?dzeniu, ani na serwerach Apple. A w chwili dokonywania p?atno?ci Apple nigdy nie udost?pnia numer車w kart sprzedawcom

Us?uga wymaga wcze?niejszego podpisania umowy z operatorem kartowym (po szczeg車?owe informacje nale?y kierowa? pytania do Dzia?u Handlowego Przelewy24).

Tworzenie Merchant ID i certyfikacja domeny
Procesowanie transakcji poprzez ApplePay wymaga od Sprzedawcy utworzenia merchanta, oraz przej?cia certyfikacji domeny. Proces opisany jest pod adresem https://developer.apple.com/documentation/apple_pay_on_the_web/configuring_your_environment.

Tworzenie Apple Pay Payment Processing Certificate
Przed przyst?pieniem do procesu tworzenia certyfikatu nale?y pozyska? plik CSR (po szczeg車?owe informacje nale?y kierowa? pytania do Dzia?u Wdro?e里 Przelewy24 poprzez formularz kontaktowy).

Po zalogowaniu si? do swojego konta Apple Developer nale?y:

wybra? opcj? Certificates, IDs & Profiles,

wybra? opcj? Identifiers/Merchant IDs,

wybra? utworzony w poprzednim kroku Merchant ID i przej?? do opcji Edit,

wybra? opcj? Create Certificate w sekcji Apple Pay Payment Processing Certificate,

wybra? opcj? Continue,

za?adowa? plik CSR otrzymany od Dzia?u Wdro?e里 Przelewy24, korzystaj?c z opcji Choose File,

pobra? wygenerowany certyfikat i przekaza? go do Dzia?u Wdro?e里 Przelewy24.

Schemat komunikacji Apple Pay
Po klikni?ciu w przycisk "ApplePay" klientowi pojawia si? na ekranie formatka Apple, na kt車rej mo?e wybra? zapisan? na urz?dzeniu kart?. Skrypt w tle przekazuje zakodowane dane karty w obiekcie paymentData, kt車r? sklep musi przej??, a nast?pnie zakodowa? przez funkcj? base64 ponownie i przes?a? w parametrze methodRefId wraz z danymi transakcji. Sklep na swojej stronie musi wywo?a? skrypt udost?pniony przez Apple (szczeg車?y dost?pne pod adresem: https://developer.apple.com/documentation/apple_pay_on_the_web).

Skrypt w tle przekazuje zakodowane dane karty, kt車r? sklep musi przej??, a nast?pnie zakodowa? przez funkcj? base64 ponownie i przes?a? w parametrze methodRefId wraz z danymi transakcji.

Rejestracja transakcji
Rejestracja transakcji przez ApplePay polega na dodaniu dodatkowego parametru "methodRefId". Parametr mo?e zosta? uzyskany z Apple poprzez postMessage (paymentMethodToken.token) i powinien by? zakodowany funkcj? base64.

POST https://secure.przelewy24.pl/api/v1/transaction/register

Additional POST call parameter

Field name	Type	Required	Description
methodRefId	STRING	Y	Token otrzymany z ApplePay zakodowany funkcj? base64
Informacje dodatkowe
W celu usp車jnienia stylistyki stosowanej na stronie www oraz w aplikacji mobilnej dost?pne s? wskaz車wki, kt車rych nale?y si? trzyma? przy wdra?aniu us?ugi ApplePay. Opisy styli oraz oraz przycisk車w dla stron www dostepne s? pod adresem https://developer.apple.com/apple-pay/acceptable-use-guidelines-for-websites/.

Apple Pay API
Przekazanie tokenu
Po otrzymaniu tokenu z Przelewy24 nale?y na stronie wywo?a? skrypt w JS.

Skrypt w Java Script procesuje transakcje, po jej wykonaniu wywo?ywana jest odpowiednia odpowied? zwrotna (callback).

Path Parameters:

TOKEN any
Token uzyskany z Przelewy24.

GET /bundle/applepay/{TOKEN}

Request samples
HTML

html
<head>
    <meta charset="UTF-8">
    <meta name="viewport"
          content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Apple Pay</title>
    <script src="https://secure.przelewy24.pl/bundle/applepay/{{TOKEN}}"></script>
</head>
<body>
<script>
    document.addEventListener("DOMContentLoaded", function () {
      Przelewy24ApplePay.config({
        errorCallback: function () {
            alert("error");
        },
        exceptionCallback: function () {
            alert("exception");
        },
        requestFailedCallback: function () {
            alert('requestFailed');
        },
        completePaymentCallback: function () {
            alert("success");
        }
      });
    Przelewy24ApplePay.charge();
    });
  </script>
</body>
Buttony i bannery
Raty - wst?p
Rozwi?zania finansuj?ce mog? by? sposobem na zwi?kszenie obrot車w i zmniejszenie odsetka porzucanych koszyk車w w Twoim sklepie. Jednak samo w??czenie rat online na formatce p?atniczej nie zadzia?a (albo zadzia?a w＃ mocno ograniczonym zakresie).

Kluczem do tego, by wykorzysta? potencja? rat online jest ich w?a?ciwa ekspozycja w sklepie. Z tego przewodnika dowiesz si?, jak to zrobi?.

Minimalizm to dobra praktyka w zakresie UX, ale zapomnij o nim, gdy chcesz, aby Twoi klienci wybierali p?atno?? na raty. Je?li chcesz dotrze? do kupuj?cych, kt車rzy wybieraj? t? metod? op?acenia zam車wienia, zadbaj o to, by dowiedzieli si? o niej jeszcze przed jego finalizacj?.

Informuj klient車w o mo?liwo?ci roz?o?enia p?atno?ci na raty na ka?dym etapie ?cie?ki zakupowej:

na stronie g?車wnej,

na li?cie produkt車w,

na karcie produktu oraz

w checkoucie.

Pami?taj o tym, ?e klient, kt車ry wybiera raty, zwykle dysponuje ograniczonymi ?rodkami. Jest zdecydowany na zakup, ale szuka najkorzystniejszej oferty.

Informuj o ratach ju? na stronie g?車wnej. Wykorzystaj slider lub top banner.

W ten spos車b dotrzesz do szerokiego grona klient車w. Nowym odwiedzaj?cym dasz sygna?, ?e mog? p?aci? na raty.

Dodaj logo rat w?r車d metod p?atno?ci prezentowanych w stopce strony. To jedno z podstawowych miejsc, gdzie klienci szukaj? informacji o mo?liwych sposobach op?acenia zam車wienia.

Je?li Twoja witryna jest rozbudowana(kategorie, podkategorie), umie?? przycisk ?Tu kupisz na raty§ tak?e na podstronach sklepu.

Informowane Klienta o ratach na karcie produktu pomaga zwi?kszy? konwersj? tej metody p?atno?ci do ponad 40%, podczas gdy konwersja ratalna przy informacji dopiero w koszyku czy na checkoucie to 8%. Opcja minimum to statyczne reklamy graficzne Dzi?ki nim poinformujesz klienta o dost?pno?ci us?ugi Przelewy24 Raty.

Lista produkt車w to kolejne miejsce, gdzie za pomoc? informacji o ratach mo?esz sk?oni? klienta do podj?cia szybszej decyzji zakupowej lub do dodania do koszyka dodatkowych produkt車w.

Koszyk / podsumowanie zam車wienia - tutaj zdecydowanie nie powinno zabrakn?? informacji o ratach. Ostatnia prosta. Nie chcesz, aby na tym etapie klient porzuci? koszyk. Wy?wietl jasny komunikat dotycz?cy finalizacji p?atno?ci ratalnych. Pami?taj, ?e w tym miejscu klient prawdopodobnie wybra? ju? metod? p?atno?ci, wi?c raty komunikuj mu od samego pocz?tku jego podr車?y zakupowej.

Checkout - wyci?gnij metod? p?atno?ci ratalnej do checkout.

Jak wy?wietli? dan? metod? p?atno?ci znajdziesz tutaj Jak wyswietlic w sklepie pe?en wyb車r metod p?atno?ci?
Domy?lnie metoda ratalna jest dost?pna jako method=303. Przekierowanie nale?y wykona? zgodnie z wskazanymi wytycznymi.

Wszystkie materia?y graficzne s? dost?pne tutaj (https://www.przelewy24.pl/storage/app/media/do-pobrania/p24_raty/p24_raty_materialy_graficzne.zip)

Widget
Zaprezentuj najni?sz? mo?liw? rat? dla konkretnego produktu.

Po klikni?ciu w widget mo?e pojawi? si? dowolny element np. pop-up z informacjami o Przelewy24 | Raty Aby uruchomi? widget nale?y wykona? javasript:

Wersja MINI
Przyk?ad widgetu MINI zaprezentowanego w sklepie:

Kod niezb?dny do wywo?ania powy?szego widgetu:

html
<body>
  <!-- Tutaj b?dzie wy?wietlony widget z wersj? mini -->
  <div id="installment-widget-mini"></div>
  <!-- Osadzenie tagu z paczk? -->
  <script type="application/javascript" src="https://apm.przelewy24.pl/installments/installment-calculator-app.umd.sdk.js"></script>
  <script>
    document.addEventListener("DOMContentLoaded", async () => {
      const config = {
        sign: "string",
        posid: 'test', // Identyfikator punktu p?atno?ci posId.
        method: '303',
        amount: 1000, // grosze
        currency: "PLN", // Waluta tylko "PLN"
        lang: "pl",  // na ten moment tylko pl
        test: false  // Opcjonalnie, do wykorzystania podczas test車w, przyjmuje warto?? boolean
      }
      // Utworzenie konstruktora i podanie configu
      const installmentCalculatorApp = new InstallmentCalculatorApp(config);
      // Utworzenie komponentu miniWidget
      const miniWidget = await installmentCalculatorApp.create('mini-widget');
      // Wyrenderowanie komponentu miniWidget w tagu o id installment-widget-mini
      miniWidget.render('installment-widget-mini');
    });
  </script>
</body>
Wersja MAX
Przyk?ad widgetu MAX zaprezentowanego w sklepie:

Kod niezb?dny do wywo?ania powy?szego widgetu:

html
<body>
  <!-- Tutaj b?dzie wy?wietlony widget z wersj? max -->
  <div id="installment-widget-max"></div>
  <!-- Osadzenie tagu z paczk? -->
  <script type="application/javascript" src="https://apm.przelewy24.pl/installments/installment-calculator-app.umd.sdk.js"></script>
  <script>
    document.addEventListener("DOMContentLoaded", async () => {
      const config = {
        sign: "string",
        posid: 'test', // Identyfikator punktu p?atno?ci posId.
        method: '303',
        amount: 1000, // grosze
        currency: "PLN", // Waluta tylko "PLN"
        lang: "pl",  // na ten moment tylko pl
        test: false  // Opcjonalnie, do wykorzystania podczas test車w, przyjmuje warto?? boolean
      }
      // Utworzenie konstruktora i podanie configu
      const installmentCalculatorApp = new InstallmentCalculatorApp(config);
      // Utworzenie komponentu maxWidget
      const maxWidget = await installmentCalculatorApp.create('max-widget');
      // Wyrenderowanie komponentu maxWidget w tagu o id installment-calculator-max
      maxWidget.render('installment-widget-max');
    });
  </script>
</body>
Symulator
To spos車b na zwi?kszenie zaanga?owania u?ytkownika. Kalkulator pozwala na zaprezentowanie bardziej szczeg車?owych informacji o dost?pnych ratach.

?Przeklikanie§ r車?nych opcji ratalnych zbli?y klienta do przej?cia na kolejny etap ?cie?ki zakupowej.

Przyk?ady gotowych kod車w do prezentacji symulatora na stronie
Wersja MINI z klikni?ciem i wywo?aniem kalkulatora w modalu

Wersja MAX z klikni?ciem i wywo?aniem kalkulatora w modalu

MINI widget z klikni?ciem i otrzymaniem linku do kalkulatora

MAXI widget z klikni?ciem i otrzymaniem linku do kalkulatora

W?asny button z klikni?ciem i wywo?aniem kalkulatora w modalu

W?asny button z klikni?ciem i otrzymaniem linku do kalkulatora

Wersja MINI z klikni?ciem i wywo?aniem kalkulatora w modalu
html
<body>
  <!-- Tutaj b?dzie wy?wietlony widget z wersj? mini -->
  <div id="installment-widget-mini"></div>
  <!-- Kontener na modal nale?y doda? na ko里cu body -->
  <div id="calculator-modal"></div>
  <!-- Osadzenie tagu z paczk? -->
  <script type="application/javascript" src="https://apm.przelewy24.pl/installments/installment-calculator-app.umd.sdk.js"></script>
  <script>
    document.addEventListener("DOMContentLoaded", async () => {
      const config = {
        sign: "string",
        posid: 'test', // Identyfikator punktu p?atno?ci posId.
        method: '303',
        amount: 1000, // grosze
        currency: "PLN", // Waluta tylko "PLN"
        lang: "pl",  // na ten moment tylko pl
        test: false  // Opcjonalnie, do wykorzystania podczas test車w, przyjmuje warto?? boolean
      }
      // Utworzenie konstruktora i podanie configu
      const installmentCalculatorApp = new InstallmentCalculatorApp(config);
      // Utworzenie komponentu calculatorModal
      const calculatorModal = await installmentCalculatorApp.create('calculator-modal');
      // Wyrenderowanie modal-a z kalkulatorem w tagu o id calculator-modal.
      // Uwaga!!!
      // 1. Nie u?ywa? id="installment-calculator-modal"
      // 2. calculatorModal musi by? wyrenderowany przed miniWidget
      calculatorModal.render('calculator-modal');
      // Utworzenie komponentu miniWidget
      const miniWidget = await installmentCalculatorApp.create('mini-widget');
      // Wyrenderowanie komponentu miniWidget w tagu o id installment-widget-mini
      miniWidget.render('installment-widget-mini');
    });
  </script>
</body>
Parametry niezb?dne do wywo?ania widgetu lub symulatora:
Parametr	Opis
sign	Wyliczony jako sha384({?crc§:§string§,§posId§:int,§method§:int})
posId	ID Sklepu (domy?lnie ID Partnera)
method	Domy?lnie 303
amount	Kwota wyra?ona w groszach
lang	Dozwolone: pl
currency	Dozwolone: PLN
Parametry opcjonalne do wywo?ania widgetu lub symulatora:
Parametr	Opis
test	Dozwolone: true, false
Wys?any parametr test z warto?ci? true sprawia, ?e pola sign, posId oraz method nie s? walidowane, musz? zosta? natomiast przekazane w obiekcie config.
