CREATE STREAM RawStream_AspNetRoleClaims (
  after STRUCT<Id VARCHAR, RoleId VARCHAR, ClaimType VARCHAR, ClaimValue VARCHAR>)
  WITH (
    KAFKA_TOPIC = 'iddb.public.AspNetRoleClaims',
    VALUE_FORMAT = 'JSON');

CREATE STREAM KeyedStream_AspNetRoleClaims WITH (
    KAFKA_TOPIC = 'ksql-stream_iddb.public.AspNetRoleClaims',
    VALUE_FORMAT = 'JSON') AS
  SELECT
    after->Id as Id,
    after->RoleId as RoleId,
    after->ClaimType as ClaimType,
    after->ClaimValue as ClaimValue
  FROM RawStream_AspNetRoleClaims
  WHERE after->Id <> '-1'
  PARTITION BY Id;
