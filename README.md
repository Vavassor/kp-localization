# KP Localization

KP Localization is a package to help VRChat world creators support multiple languages.

## Limitations

### Date, Time, and Number Formatting

Udon doesn't give access to create CultureInfo objects. So culture-specific formatting will always follow the language set by VRChat and can't be controlled by worlds.

### Ordinal Numbers

Ordinal numbers are like 1st, 2nd, and 3rd. Currently we don't support ordinal plural rules. LocalizedText.count assumes cardinal numbers and should not be used for ordinal numbers.
