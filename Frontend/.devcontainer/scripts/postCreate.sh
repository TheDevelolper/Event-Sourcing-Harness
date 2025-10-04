#!/usr/bin/env bash
set -e

# Ensure PATH includes dotnet tools
export PATH="$PATH:$HOME/.dotnet/tools"

# Install Aspire CLI for the current (non-root) user
dotnet tool install -g Aspire.Cli --prerelease || dotnet tool update -g Aspire.Cli

# Clear NuGet caches
dotnet nuget locals all --clear

# Go to the correct directory for restore/build
cd dotnet

# Restore and clean the main solution
dotnet restore SaasFactory.sln
dotnet clean SaasFactory.sln

# Trust HTTPS dev certs
dotnet dev-certs https --trust
