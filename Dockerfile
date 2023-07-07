#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Hospital_Appointment_Booking_System/Hospital_Appointment_Booking_System.csproj", "Hospital_Appointment_Booking_System/"]
#COPY *.csproj ./
RUN dotnet restore "Hospital_Appointment_Booking_System/Hospital_Appointment_Booking_System.csproj"

#RUN dotnet restore "/Hospital_Appointment_Booking_System/Hospital_Appointment_Booking_System.csproj"
COPY . .
WORKDIR "/src/Hospital_Appointment_Booking_System"
RUN dotnet build "Hospital_Appointment_Booking_System.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hospital_Appointment_Booking_System.csproj" -c Release -o /app/publish /p:UseAppHost=false
RUN echo 'Hello World!' > /html/index.html

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hospital_Appointment_Booking_System.dll"]

