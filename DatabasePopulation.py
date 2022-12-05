import pandas as pd
import mysql.connector

fileName = input("File Name: ")
numberOfSupermarkets = int(input("Number of Supermarkets: "))

baseCB = pd.read_excel(fileName, "BaseCB")
baseVolume = pd.read_excel(fileName, "BaseVolume")
basePeso = pd.read_excel(fileName, "BasePeso")

resultadoPeso = baseCB.merge(basePeso, on='Item')
resultadoVolume = baseCB.merge(baseVolume, on='Item')
print(resultadoPeso)
print(resultadoVolume)


print("Database Information")
host = input("Host: ")
user = input("User: ")
password = input("Password: ")
database = input("Database: ")

db = mysql.connector.connect(
    host=host,
    user=user,
    password=password,
    database=database
)
cursor = db.cursor()

val = []
sql1 = "INSERT INTO Products ( Code, Name, Price, WeightOrVolume, SupermarketId, HasVolume)" \
              "VALUES (%s, %s, %s, %s, %s, %s)"

for i in range(0, len(resultadoPeso)):
    for j in range(0, numberOfSupermarkets):
        val.append((resultadoPeso['CB'][i], resultadoPeso['Item'][i], resultadoPeso['Preço'][i], resultadoPeso['kg/embalagem'][i], j+1, 0))
        total = i + j

cursor.executemany(sql1, val)


val = []
sql2 = "INSERT INTO Products (Code, Name, Price, SupermarketId, HasVolume)" \
              "VALUES (%s, %s, %s, %s, %s)"

for i in range(0, len(resultadoVolume)):
    for j in range(0, numberOfSupermarkets):
        val.append((resultadoVolume['CB'][i], resultadoVolume['Item'][i], resultadoVolume['Preço por produto'][i], j+1, 1))

cursor.executemany(sql2, val)


db.commit()

"""sql = "INSERT INTO customers (name, address) VALUES (%s, %s)"
val = ("John", "Highway 21")
mycursor.execute(sql, val)

mydb.commit()"""


