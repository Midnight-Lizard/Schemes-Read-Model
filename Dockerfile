#===========================================#
#               DOTNET  BUILD               #
#===========================================#
FROM microsoft/dotnet:2.2-sdk as dotnet-build
ARG DOTNET_CONFIG=Release
WORKDIR /build
COPY *.sln .
COPY /app/MidnightLizard.KafkaConnect.ElasticSearch.csproj ./app/
RUN dotnet restore
COPY . .
RUN dotnet publish app -c ${DOTNET_CONFIG} -o ./results

#===========================================#
#               DOTNET  TEST                #
#===========================================#
FROM microsoft/dotnet:2.2-sdk as dotnet-test
WORKDIR /test
COPY --from=dotnet-build /build .
RUN dotnet test -c Test

#===========================================#
#               IMAGE   BUILD               #
#===========================================#
FROM microsoft/dotnet:2.2-aspnetcore-runtime as image
ARG INSTALL_CLRDBG
RUN bash -c "${INSTALL_CLRDBG}"
WORKDIR /app
EXPOSE 80
COPY --from=dotnet-build /build/app/results .
ENTRYPOINT ["dotnet", "MidnightLizard.KafkaConnect.ElasticSearch.dll"]
