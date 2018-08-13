CREATE TABLE AspNetUsers (
    UserId VARCHAR,
    UserName VARCHAR,
    DisplayName VARCHAR,
    NormalizedUserName VARCHAR,
    Email VARCHAR,
    NormalizedEmail VARCHAR,
    EmailConfirmed BOOLEAN,
    PhoneNumber VARCHAR,
    PhoneNumberConfirmed BOOLEAN,
    RoleId VARCHAR,
    RoleName VARCHAR,
    NormalizedRoleName VARCHAR,
    UserClaimType VARCHAR,
    UserClaimValue VARCHAR)
  WITH (
    KAFKA_TOPIC = 'ksql-table_iddb.public.AspNetUserClaims',
    VALUE_FORMAT = 'JSON',
    KEY = 'UserId');