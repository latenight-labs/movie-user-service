-- Script para criação do banco de dados PostgreSQL
-- Execute este script como superusuário do PostgreSQL

-- Criar o banco de dados
CREATE DATABASE movieuserservice
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.utf8'
    LC_CTYPE = 'en_US.utf8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- Criar usuário específico para a aplicação (opcional)
CREATE USER movieuser WITH PASSWORD 'moviepass123';

-- Conceder privilégios ao usuário
GRANT ALL PRIVILEGES ON DATABASE movieuserservice TO movieuser;

-- Conectar ao banco criado e conceder privilégios no schema public
\c movieuserservice;
GRANT ALL ON SCHEMA public TO movieuser;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO movieuser;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO movieuser;