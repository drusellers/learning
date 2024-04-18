#!/bin/bash

set -e
set -u

####
# Notes:
#   superuser and createdb access faciliate unit testing more than anything

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-SQL
    CREATE USER learning WITH PASSWORD 'learning' superuser createdb;
    CREATE DATABASE learning;
    GRANT ALL PRIVILEGES ON DATABASE learning TO learning;
SQL
