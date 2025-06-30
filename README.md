# HaarpTech_Licenta

Buna ziua ! 

Aici este o parte din lucrarea mea de licenta

Tehnologii folosite : ASP.NET Core MVC(dotnet9) , Microsoft SQL Server , JavaScript(fara framework) ,  HTML si CSS.

Aplicatia foloseste EF Core si Identity pentru zona de LogIn Register si UserManagement.

Toata logica este orientata in zona de DB.

Toate operatiile de CRUD (CREATE , READ ,UPDATE , DELETE) sunt scrise in Dapper care au la baza o procedura stocata.(DenumireTabel_PKG_INSERT / DenumireTabel_PKG_UPDATE / DenumireTabel_PKG_DELETE) 

Toate datele selectate din tabele se fac pe baza view-urilor (din motive de securitate)

# Acest link nu mai este valabil . am mai facut cateva modificari minore legate de DB .Aici atasez link-ul de la Schema DB-ului pentru a fi mai usor de vizualizat de dumneavoastra : https://dbdiagram.io/d/67db242275d75cc844bbe248
# Link DB Actualizat : https://dbdiagram.io/d/67db242275d75cc844bbe248

Momentan am facut :
 - partea de MeniuUtilizator , Roluri utilizator si Roluri functionala cu toate operatiile de CRUD
 - Partea de ProgramareSedinte cu toate operatiile de CRUD + un simplu searchBar
 - Partea de RaportCerinte  cu toate operatiile de CRUD + un simplu searchBar
 - Partea de CereriOferte cu toate operatiile de CRUD + un simplu searchBar


Poza LogIn
![image](https://github.com/user-attachments/assets/153f9a13-8d32-4f2c-9a46-965ebf075d83)
Se regaseste o parola dedesupt doar pentru teste , la finalizarea lucrarii o sa dispara

Poza Register
![image](https://github.com/user-attachments/assets/7d144816-6f03-40b0-a85e-e195be6e7b4f)

Poza Navbar UserAdmin autentificat
![image](https://github.com/user-attachments/assets/bda5462e-ce5e-43ca-9ece-35fb41a64965)

Poza Meniu Utilizatori
![image](https://github.com/user-attachments/assets/94223141-3fcb-4c80-a845-6d211a2029fb)

Poza Administrare Roluri
![image](https://github.com/user-attachments/assets/69cf91a4-4dc9-4618-8819-59c00ffa5654)

Poza Roluri 
![image](https://github.com/user-attachments/assets/c7798d6f-34e2-4a49-9b9b-586225ef20cd)

Poza CereriOferte(Un utilizator poate selecta un produs existent sau poate sa propuna un produs personalizat pe care doar el in poate vedea)
![image](https://github.com/user-attachments/assets/c2f5e0e3-b5bf-4102-8280-aa90a40500b5)

Poza CerereOferta
![image](https://github.com/user-attachments/assets/ec32bfd8-f73d-4731-8b42-a49134239895)

Poza Lista Sedinte
![image](https://github.com/user-attachments/assets/7781d7f6-7fce-4eff-9791-86ec0bbd1297)

Poza Detalii Sedinta
![image](https://github.com/user-attachments/assets/881308bf-6191-4287-a0c5-8b47730b684a)

Poza Detalii Raport Cerinte 
![image](https://github.com/user-attachments/assets/01fa5748-af89-46ea-b295-e1a2ccba4a53)


# Modificari facute de la ultima Sedinta pana in Prezent 

Pagina de start 

# Am adaugat o animatie care mimeaza ploaia dupa care rasoare un soare(totul dureaza 6 secunde)
![image](https://github.com/user-attachments/assets/c32d410f-2e15-40da-a77d-9cd4ec4aa7e0)


# dupa ce ne logam cu un cont de intern (admin , project manager , departament financiar sau consultant ) avem un slide bar cu functii pentru fiecare rol (acum sunt cu contul de admin)
![image](https://github.com/user-attachments/assets/f5040111-d9aa-41d4-8b00-ecb9cc070488)


# zona de Cerinte alocate fiecarui produs
![image](https://github.com/user-attachments/assets/cea0a0b0-b9b6-424b-ba8e-46e8dd1e4e35)
![image](https://github.com/user-attachments/assets/81c8b026-1ed7-4ea6-a502-5a4c1a1d71bc)
![image](https://github.com/user-attachments/assets/eabd825f-bfdc-4d1d-9992-15ac3465c2cc)

 # Pagina de tichete 
![image](https://github.com/user-attachments/assets/625dacf8-2db4-4961-a553-ce1fdcf6c037)
![image](https://github.com/user-attachments/assets/e6750446-1ffb-4903-8d95-eef027bdb8db)


# Nota de comanda
![image](https://github.com/user-attachments/assets/4b62a4b5-c708-4fab-a257-d51554e24c14)
![image](https://github.com/user-attachments/assets/4b079acd-3701-41da-be15-1bbdfa1ac3b2)
![image](https://github.com/user-attachments/assets/37668fc9-8739-45d9-9789-56b0b58ae75b)


# Lista de contracte
![image](https://github.com/user-attachments/assets/ca75db9f-dbb6-43cb-a51b-a3ce9c9b42af)
![image](https://github.com/user-attachments/assets/4d60f36f-060c-4c55-8e6c-28dbfb34e5b4)
![image](https://github.com/user-attachments/assets/99e3caeb-d833-4cd6-bb5e-5edd1c113092)


# date client facturare
![image](https://github.com/user-attachments/assets/62eadd27-d567-4e13-9722-0166f07ae8af)


# facturi
![image](https://github.com/user-attachments/assets/8a95f1f3-a2b6-4ab1-bf11-32d0419b4343)
![image](https://github.com/user-attachments/assets/b7caf48f-e56e-4d5f-8124-bf7b789da065)
![image](https://github.com/user-attachments/assets/81b3d4c4-e940-414e-91c3-32fe3702edcc)


# "Ai uitat parola"
Nu mi-a iesit sa fac un serviciu care sa schimbe parola utilizatorului , dar am adaugat acest mesaj pentru a specifica acest lucru
![image](https://github.com/user-attachments/assets/dbb96b66-97bd-444e-83d8-49ca2d96bc4d)



# Toate drepturile sunt rezervate
