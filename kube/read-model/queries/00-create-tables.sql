CREATE TABLE AspNetRoles (Id VARCHAR, Name VARCHAR, NormalizedName VARCHAR)
  WITH (
    KAFKA_TOPIC = 'ksql-stream_iddb.public.AspNetRoles',
    VALUE_FORMAT = 'JSON',
    KEY = 'Id');

CREATE TABLE AspNetUsers (
    Id VARCHAR,
    UserName VARCHAR,
    DisplayName VARCHAR,
    NormalizedUserName VARCHAR,
    Email VARCHAR,
    NormalizedEmail VARCHAR,
    EmailConfirmed BOOLEAN,
    PhoneNumber VARCHAR,
    PhoneNumberConfirmed BOOLEAN)
  WITH (
    KAFKA_TOPIC = 'ksql-stream_iddb.public.AspNetUsers',
    VALUE_FORMAT = 'JSON',
    KEY = 'Id');

CREATE TABLE AspNetUserRoles (Id VARCHAR, UserId VARCHAR, RoleId VARCHAR)
  WITH (
    KAFKA_TOPIC = 'ksql-stream_iddb.public.AspNetUserRoles',
    VALUE_FORMAT = 'JSON',
    KEY = 'Id');