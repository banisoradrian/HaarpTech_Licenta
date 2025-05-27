# HaarpTech_Licenta

Buna ziua ! 

Aici este o parte din lucrarea mea de licenta

Tehnologii folosite : ASP.NET Core MVC(dotnet9) , Microsoft SQL Server , JavaScript(fara framework) ,  HTML si CSS.

Aplicatia foloseste EF Core si Identity pentru zona de LogIn Register si UserManagement.

Toata logica este orientata in zona de DB.

Toate operatiile de CRUD (CREATE , READ ,UPDATE , DELETE) sunt scrise in Dapper care au la baza o procedura stocata.(DenumireTabel_PKG_INSERT / DenumireTabel_PKG_UPDATE / DenumireTabel_PKG_DELETE) 

Toate datele selectate din tabele se fac pe baza view-urilor (din motive de securitate)

Aici atasez link-ul de la Schema DB-ului pentru a fi mai usor de vizualizat de dumneavoastra : https://dbdiagram.io/d/67db242275d75cc844bbe248


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





