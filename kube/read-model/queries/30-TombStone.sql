CREATE TABLE Table_PublicSchemes_WithUsers (
  Id VARCHAR,
  EventType VARCHAR,
  SchemaVersion VARCHAR,
  Generation INTEGER,
  ColorScheme STRUCT<
    colorSchemeId VARCHAR,
    colorSchemeName VARCHAR,
    runOnThisSite BOOLEAN,
    blueFilter INTEGER,
    doNotInvertContent BOOLEAN,
    mode VARCHAR,
    modeAutoSwitchLimit INTEGER,
    useDefaultSchedule BOOLEAN,
    scheduleStartHour INTEGER,
    scheduleFinishHour INTEGER,
    includeMatches VARCHAR,
    excludeMatches VARCHAR,
    backgroundSaturationLimit INTEGER,
    backgroundContrast INTEGER,
    backgroundLightnessLimit INTEGER,
    backgroundGraySaturation INTEGER,
    backgroundGrayHue INTEGER,
    backgroundReplaceAllHues BOOLEAN,
    backgroundHueGravity INTEGER,
    buttonSaturationLimit INTEGER,
    buttonContrast INTEGER,
    buttonLightnessLimit INTEGER,
    buttonGraySaturation INTEGER,
    buttonGrayHue INTEGER,
    buttonReplaceAllHues BOOLEAN,
    buttonHueGravity INTEGER,
    textSaturationLimit INTEGER,
    textContrast INTEGER,
    textLightnessLimit INTEGER,
    textGraySaturation INTEGER,
    textGrayHue INTEGER,
    textSelectionHue INTEGER,
    textReplaceAllHues BOOLEAN,
    textHueGravity INTEGER,
    linkSaturationLimit INTEGER,
    linkContrast INTEGER,
    linkLightnessLimit INTEGER,
    linkDefaultSaturation INTEGER,
    linkDefaultHue INTEGER,
    linkVisitedHue INTEGER,
    linkReplaceAllHues BOOLEAN,
    linkHueGravity INTEGER,
    borderSaturationLimit INTEGER,
    borderContrast INTEGER,
    borderLightnessLimit INTEGER,
    borderGraySaturation INTEGER,
    borderGrayHue INTEGER,
    borderReplaceAllHues BOOLEAN,
    borderHueGravity INTEGER,
    imageLightnessLimit INTEGER,
    imageSaturationLimit INTEGER,
    useImageHoverAnimation BOOLEAN,
    backgroundImageLightnessLimit INTEGER,
    backgroundImageSaturationLimit INTEGER,
    scrollbarSaturationLimit INTEGER,
    scrollbarContrast INTEGER,
    scrollbarLightnessLimit INTEGER,
    scrollbarGrayHue INTEGER,
    scrollbarSize INTEGER,
    scrollbarStyle VARCHAR>,
  Name VARCHAR,
  Side VARCHAR,
  PublisherId VARCHAR,
  PublisherName VARCHAR,
  PublisherCommunity VARCHAR) WITH (
    KAFKA_TOPIC = 'ksql-stream_read-model_public-schemes',
    VALUE_FORMAT = 'JSON',
    KEY = 'ID');

CREATE TABLE Table_PublicSchemes_WithTombstone WITH (
    KAFKA_TOPIC = 'ksql-table_read-model_public-schemes',
    VALUE_FORMAT = 'JSON') AS
  SELECT
    Id, SchemaVersion, Name, Side, ColorScheme,
    PublisherId, PublisherName, PublisherCommunity,
    Generation
  FROM Table_PublicSchemes_WithUsers
  WHERE EventType = 'SchemePublishedEvent';