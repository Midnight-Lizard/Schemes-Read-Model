{{/* vim: set filetype=mustache: */}}
{{/*
Expand the name of the chart.
*/}}
{{- define "name" -}}
{{- .Release.Name | trunc 63 | trimSuffix "-" -}}
{{- end -}}
