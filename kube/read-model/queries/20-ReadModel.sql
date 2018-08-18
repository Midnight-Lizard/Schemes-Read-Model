CREATE STREAM PublicSchemes_WithUsers WITH (
    KAFKA_TOPIC = 'ksql-stream_read-model_public-schemes',
    VALUE_FORMAT = 'JSON') AS
  SELECT
    event.Payload->AggregateId as Id,
    event.Type as EventType,
    event.Version as SchemaVersion,
    event.Payload->ColorScheme as ColorScheme,
    event.Payload->ColorScheme->colorSchemeName as Name,
    EXTRACTJSONFIELD('[\"dark\", \"light\"]', '$[' + CAST(ROUND(CAST(event.Payload->ColorScheme->backgroundLightnessLimit as DOUBLE) / 100) as VARCHAR) + ']') as Side,
    user.UserId as PublisherId,
    user.DisplayName as PublisherName,
    user.NormalizedRoleName <> 'OWNER' as PublisherCommunity
  FROM Stream_PublicSchemes event
  LEFT JOIN AspNetUsers user ON user.UserId = event.UserId
  PARTITION BY Id;
