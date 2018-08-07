-- dropping rekeyed streams
DROP STREAM IF EXISTS KeyedStream_AspNetRoleClaims DELETE TOPIC;
DROP STREAM IF EXISTS KeyedStream_AspNetRoles DELETE TOPIC;
DROP STREAM IF EXISTS KeyedStream_AspNetUserClaims DELETE TOPIC;
DROP STREAM IF EXISTS KeyedStream_AspNetUserLogins DELETE TOPIC;
DROP STREAM IF EXISTS KeyedStream_AspNetUserRoles DELETE TOPIC;
DROP STREAM IF EXISTS KeyedStream_AspNetUsers DELETE TOPIC;
-- dropping raw streams
DROP STREAM IF EXISTS RawStream_AspNetRoleClaims;
DROP STREAM IF EXISTS RawStream_AspNetRoles;
DROP STREAM IF EXISTS RawStream_AspNetUserClaims;
DROP STREAM IF EXISTS RawStream_AspNetUserLogins;
DROP STREAM IF EXISTS RawStream_AspNetUserRoles;
DROP STREAM IF EXISTS RawStream_AspNetUsers;
--------------------------
