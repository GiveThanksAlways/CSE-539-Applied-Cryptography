FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic

RUN apt-get update
RUN apt-get install zip -y

WORKDIR /home
# COPY ./P1_1/P1_1.zip .
# COPY ./P1_2/P1_2.zip .
COPY ./P3/P3.zip .

# dotnet run "B1 FF FF CC 98 80 09 EA 04 48 7E C9"

# dotnet run "Hello World" "RgdIKNgHn2Wg7jXwAykTlA=="