# Realisierungskonzept
## Nutzen
Unsere Applikation soll es erleichtern, seine Dokumente zu bearbeiten, aber auch gleichzeitig mit drittpersonen teilen zu können.
Der Grösste Nutzen besteht darin, dass unsere Applikation es erlaubt, einer Person ein Dokument freizugeben, welche gar keinen Account besitzt. Damit kann man auch externe Meinungen ganz einfach abholen.

### Zielgruppe
Sie ist für Personen, welche gerne schreiben konzipiert. Man kann Dokumente hochladen wenn man möchte, sodass man sie auf anderen Geräten auch direkt zur verfügung hat, oder sie Lokal gespeichert lassen.
Das Heruntergeladene Dokument kann dann ganz einfach auch Offline bearbeitet werden, und wenn man wieder eine verbindung zum Internet hat hochgeladen werden. 
Man benötigt keinen Account, um ein Dokument von einer anderen Person zu sehen, solange diese Person das Dokument freigegeben hat.

## Systemarchitektur
**Frontend:**
Das Frontend ist dafür da, dass unsere Benutzer eine einfache und gute Zeit haben, beim benutzen des Produktes. Es hat eine direkte Verbindung zum Backend.

**Backend:**
Das Backend bietet die API-Endpunkte an, um die CRUD Operationen über die Datenbank ausführen zu können. Es erlaubt nur dem Frontend CORS anfragen zu senden.

**Datenbank:**
Die Datenbank welche nur eine Verbindung zulässt (zum Backend) ist dafür da, die Benutzerdaten, sowie die Pfade zu den Dokumenten zu speichern.

**Authentifizierung:**
Dieser Teil ist im Backend direkt integriert.
Dadurch wird sichergestellt, dass nur Authentifizierte Personen zugriff auf das Backend und somit auch Datenbank haben.

**Authorisierung:**
So wie die Authentifizierung wird auch die Authorisierung direkt ins Backend integriert. 
Diese stellt sicher, dass Authentifizierte User nur auf das zugreifen können, auf das sie die Rechte haben.

## Technologie
Unser Backend wird mit Asp.Net umgesetzt, die Daten werden mit einer Postgres Datenbank Persistent gemacht. 
Unser Frontend, welches mit Angular erstellt wird, hat eine Verbindung zur Rest API im Backend. 
Das Backend erlaubt CORS nur über die Spezifische Adresse des Frontends und von sich selbst, sodass unerlaubte nicht Daten verändern können. 
Um Sicherzustellen, dass nur Personen mit genügend Rechten wichtige Daten löschen / verändern können benutzen wir die direkt in ASP.Net Core eingebaute Rollenbasierte Authorisierung.
Die Authentifizierung wird mit JWT Token umgesetzt.

Authorisierung: [Microsoft Learn - Authorization](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-9.0)
Authentifizierung: [Microsoft Learn - Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-9.0) und [JWT Token](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-jwt-bearer-authentication?view=aspnetcore-9.0)

## Arbeitszyklus
Die Arbeit welche wir im Team erarbeiten wird auf github immer wieder integriert. Durch eine CI/CD Pipeline wird der Code automatisch geprüft und auf test oder prod hochgeladen, wenn es auf dem main oder test branch ist.
Änderungen am Projekt müssen mindestens von einer anderen Person reviewed werden mit einem PR. Auf main kann nur von test aus gemerged werden.

### Deployment
Unsere Test Applikation sowie die scharfe Applikation laufen beide auf Azure. Das Deployment wird automatisch über github actions erledigt.
Die Codequalität wird im gleichen schritt auch noch geprüft, sodass Fehler frühzeitig entdeckt werden.


## Abhängigkeiten
Unser Projekt besitzt eine direkte Abhängigkeit zu Azure, da wir unsere Applikation dort hosten. Wenn Azure abstürzt, würde unsere App dadurch auch nicht mehr funktionieren.
Vom Code her sind wir darauf angewiesen, dass einmal C# .net weiterentwickelt wird, und auch dass die Abhängigkeiten, welche wir direkt in unserem Projekt eingebunden haben geupdated werden. Wäre das nicht der fall, könnten wir im Falle einer kleineren Abhängigkeit einfach ein anderes Nuget verwenden.

## Datenmodell

![img_2.png](img_2.png)

Dies ist das Modell für die erste Version der Applikation. Es ist reduziert auf das minimum.

### Datenfluss

#### Dokument erstellen / hochladen

Frontend sendet Datei + Metadaten an Backend

Backend prüft JWT (falls vorhanden)

Datei wird in Azure Storage gespeichert

Metadaten werden in Postgres persistiert

#### Dokument teilen

Authentifizierter User erstellt ShareLink

Backend generiert Token und speichert ihn

Link wird an externe Person weitergegeben

#### Zugriff ohne Account

Externer Nutzer ruft ShareLink auf

Backend validiert Token, Rechte und Ablaufdatum

Dokument wird lesend oder schreibend freigegeben

#### Offline-Bearbeitung

Dokument wird lokal gespeichert

Änderungen werden später synchronisiert

Konflikte werden über Versionsnummer erkannt

### DSGVO-Konzept

#### Datensparsamkeit:
Es werden nur notwendige personenbezogene Daten gespeichert (E-Mail, Authentifizierungsdaten).

#### Anonyme Nutzung:
Externe Nutzer benötigen kein Konto, es werden keine personenbezogenen Daten erhoben.

#### Recht auf Löschung:
Benutzer können ihre Dokumente und Accounts vollständig löschen.

#### Logging:
Keine Bewegungsprofile, es wird nur das Nötigste geloggt um einen Verlauf nachvollziehen zu können, ohne schlüsse auf den User führen zu können.

## Sicherheitskonzept
Das Sicherheitskonzept der Applikation basiert auf einer JWT-basierten Authentifizierung und einer rollenbasierten Autorisierung mit ASP.NET Core, 
wodurch sichergestellt wird, dass Benutzer nur auf die für sie vorgesehenen Funktionen und Daten zugreifen können. Dokumente können über ShareLinks geteilt werden, 
die zeitlich begrenzt sind und mit eingeschränkten Rechten versehen werden können, um den Zugriff externer Personen kontrollierbar zu halten. 
Zusätzlich schützt sich das System durch den Einsatz eines ORMs mit Parameterbindung vor SQL-Injection sowie durch konsequentes Sanitizing von Eingaben im Frontend vor Cross-Site-Scripting (XSS).

## UI
### Dateiablagerungsansicht
![Hallo](img.png)
Dieses UI zeigt die Dokumentenübersicht und ist auf schnelle Orientierung und einfache Verwaltung ausgelegt. 
Auf der linken Seite befindet sich eine Baumstruktur zur Navigation durch Ordner und Kategorien, 
getrennt nach privaten und freigegebenen Inhalten. Dadurch lassen sich Dokumente logisch strukturieren und schnell wiederfinden.

Der Hauptbereich stellt die Dokumente als Kachelansicht dar. 
Unterschiedliche Symbole und Statusanzeigen zeigen auf einen Blick, 
ob ein Dokument lokal, in der Cloud gespeichert oder freigegeben ist. 
Das Layout ist klar und konsistent gehalten, mit neutralen Farben und einer dezenten Akzentfarbe, 
um den Fokus auf die Inhalte und deren Status zu legen.

### Bearbeitungsmodus
![img_1.png](img_1.png)
Das UI-Design ist als einfacher, übersichtlicher Dokumenten-Editor umgesetzt.
Der Fokus liegt klar auf dem Dokument selbst, das zentral auf einer hellen Arbeitsfläche dargestellt wird,
um eine gute Lesbarkeit zu gewährleisten und Ablenkungen zu vermeiden.

Die wichtigsten Textformatierungen befinden sich in einer seitlichen Werkzeugleiste und sind jederzeit schnell erreichbar.
Globale Aktionen sind in einer oberen Navigationsleiste zusammengefasst. Das Layout ist klar strukturiert, mit neutralen Farben und einer gut lesbaren Standardschrift,
sodass sich die Oberfläche intuitiv und ruhig bedienen lässt.

## Ressourcen
Die Projektorganisation ist bewusst schlank gehalten. Das Projekt wird von zwei Personen umgesetzt, 
die beide für die Entwicklung von Frontend und Backend verantwortlich sind. 
Weitere personelle Ressourcen stehen nicht zur Verfügung. Die Aufgaben werden gemeinsam geplant und aufgeteilt, 
wobei beide Entwickler flexibel in mehreren Bereichen arbeiten.

Da sich das Team in der Lehre befindet, stehen nur begrenzte finanzielle Mittel zur Verfügung. 
Aus diesem Grund werden hauptsächlich kostenlose oder kostengünstige Technologien und Services eingesetzt, 
wie Open-Source-Frameworks sowie Cloud-Angebote mit Free- oder Student-Tarifen. 
Der Fokus liegt auf effizienter Ressourcennutzung, klarer Aufgabenverteilung und automatisierten Prozessen, 
um den Entwicklungsaufwand möglichst gering zu halten.

## Zeitplan
### Woche 1 – Konzeption und Setup
Anforderungsanalyse, Finalisierung der Architektur, Definition des Datenmodells, Erstellung des Repositories sowie Einrichtung der CI/CD-Pipeline und der Azure-Testumgebung.
Meilenstein: Entwicklungsumgebung lauffähig, Konzept abgeschlossen.

### Woche 2–3 – Backend-Entwicklung
Umsetzung der REST-API mit ASP.NET Core, Implementierung der Authentifizierung (JWT) und der rollenbasierten Autorisierung, Anbindung der Postgres-Datenbank sowie von Azure Storage.
Meilenstein: Gesichertes Backend mit funktionsfähigen CRUD-Endpunkten.

### Woche 4–5 – Frontend-Entwicklung
Entwicklung des Angular-Frontends mit Dokumentenübersicht, Upload- und Download-Funktionalität sowie grundlegender Bearbeitung und API-Anbindung.
Meilenstein: Zentrale Funktionen über das Frontend nutzbar.

### Woche 6 – Teilen und Offline-Funktionalität
Implementierung von ShareLinks für externe Nutzer, Offline-Bearbeitung und Synchronisation inklusive Versionsverwaltung.
Meilenstein: Dokumente können geteilt und offline bearbeitet werden.

### Woche 7 – Testphase und Fehlerbehebung
Durchführung funktionaler Tests, Sicherheitsprüfungen, Bugfixes sowie Stabilisierung des Systems in der Testumgebung.
Meilenstein: Release-Kandidat bereit.

### Woche 8 – Deployment und Go-live
Deployment in die produktive Azure-Umgebung, Abschlusskontrollen und Start des produktiven Betriebs.
Meilenstein: Go-live der Applikation.

### Ab Woche 9 – Wartung und Weiterentwicklung
Kontinuierliche Wartung, Bugfixes, Sicherheitsupdates und kleinere Erweiterungen ohne festen Release-Zyklus.

## Sicherheitsanalyses
Bei der Applikation ergeben sich mehrere sicherheitsrelevante Risiken, die sowohl technischer als auch organisatorischer Natur sind. Ein zentrales Risiko liegt im Teilen von Dokumenten über ShareLinks, da diese auch von Personen ohne Benutzerkonto verwendet werden können. Trotz zeitlicher Begrenzung und Rechtevergabe besteht die Gefahr, dass Links unkontrolliert weitergegeben oder abgefangen werden, was zu unautorisiertem Zugriff auf Dokumente führen kann.

Weitere Risiken betreffen die Authentifizierung und Autorisierung im Backend. Fehlerhafte Konfigurationen der JWT-Validierung oder der rollenbasierten Zugriffskontrolle könnten dazu fuehren, dass Benutzer auf fremde Dokumente oder administrative Funktionen zugreifen können. Auch bei der Offline-Bearbeitung besteht ein erhöhtes Risiko von Versionskonflikten, wodurch änderungen ueberschrieben oder verloren gehen können, insbesondere bei paralleler Bearbeitung durch mehrere Personen.

Auf infrastruktureller Ebene stellt die starke Abhängigkeit von Azure ein wesentliches Risiko dar. Ein Ausfall von Azure-Diensten oder von Azure Storage wuerde die Verfuegbarkeit der gesamten Applikation beeinträchtigen. Da keine Hochverfuegbarkeits- oder Fallback-Strategie vorgesehen ist, wird dieses Risiko bewusst akzeptiert. Zusätzlich können Fehlkonfigurationen bei CORS oder bei den Zugriffsrechten auf Storage und Datenbank potenzielle Angriffsflächen eröffnen.

Organisatorisch ergeben sich Risiken aus der sehr kleinen Teamgrösse und den begrenzten zeitlichen Ressourcen. Sicherheitsupdates, Abhängigkeits-Updates oder Reaktionen auf Sicherheitsvorfälle könnten verzögert erfolgen. Da kein dediziertes Monitoring oder Incident-Response-Konzept vorhanden ist, besteht die Gefahr, dass Sicherheitsprobleme erst spät erkannt werden. Menschliche Faktoren wie schwache Passwörter oder ein unachtsamer Umgang mit ShareLinks stellen weitere Risiken dar, die technisch nur eingeschränkt verhindert werden können.

Insgesamt werden grundlegende Sicherheitsmechanismen eingesetzt, jedoch bleiben insbesondere durch ShareLinks, Cloud-Abhängigkeit und Ressourcenmangel relevante Restrisiken bestehen.

## Supportstruktur
Unser Support wird vorallem in der Form eines Discord-Kanals gehalten. 
Dort können User wenn sie fragen oder Probleme haben einerseits auf die Entwickler Zählen, 
und andererseits auch hilfe von anderen Benutzern erhalten.

Die Wartung der Anwendung erfolgt kontinuierlich nach dem Go-live. 
Dazu gehören Bugfixes, Sicherheitsupdates sowie kleinere Verbesserungen im Rahmen der verfügbaren Ressourcen.

Die Release-Planung erfolgt ereignisbasiert und nicht zeitlich fixiert. 
Bei der Umsetzung neuer Features sowie bei relevanten Bugfixes wird jeweils ein neues Release erstellt. 
Es gibt keinen festen Release-Zeitplan, da die Entwicklung projektbegleitend und im Rahmen der verfügbaren Zeit erfolgt. 
Vor jedem Release werden die Änderungen über die Testumgebung geprüft und anschliessend über die CI/CD-Pipeline in die Produktivumgebung ausgerollt.