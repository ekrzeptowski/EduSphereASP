# EduSphere

Projekt został utworzony przy pomocy [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/EduSphere)
wersja 8.0.5.

## Budowanie

Uruchom `dotnet build -tl` żeby zbudować projekt.

## Uruchomienie

Żeby uruchomić projekt, użyj `docker-compose up` w głównym katalogu projektu lub uruchom projekt lokalnie
używając `dotnet run` w katalogach głównego serwera i mikroserwisów `.\src\Web\`, `.\src\AuthAPI`, `.\src\EduAPI`.

```bash
docker-compose up
```

lub

```bash
cd .\src\AuthAPI\
dotnet watch run
```

```bash
cd .\src\EduAPI\
dotnet watch run
```

```bash
cd .\src\Web\
dotnet watch run
```

Aplikacja powinna być dostępna pod adresem https://localhost:48443 w przypadku docker-compose
lub https://localhost:44447
w przypadku uruchomienia lokalnego.

## Funkcjonalności

* Aplikacja pozwala na rejestrację, logowanie, wylogowanie.
* Użytkownik może przeglądać kursy, zapisywać się na kursy, przeglądać lekcje zapisanych kursów.
* Nauczyciel może dodawać kursy, dodawać lekcje do kursów, usuwać kursy, usuwać lekcje z kursów.
* Administrator może dodawać nauczycieli.

## Dane testowe użytkowników

* Student: `student@localhost`, hasło: `Student1!`
* Nauczyciel: `teacher@localhost`, hasło: `Teacher1!`
* Administrator: `administrator@localhost`, hasło: `Administrator1!`

## Testowanie

Żeby uruchomić testy jednostkowe oraz integracyjne, użyj `dotnet test` w głównym katalogu projektu.

```bash
dotnet test
```

## Swagger

Aby zobaczyć dostępne endpointy, uruchom aplikację i przejdź do https://localhost:48443/api
lub https://localhost:44447/api.

## Autor

- [Ewelina Krzeptowska](https://github.com/ekrzeptowski)