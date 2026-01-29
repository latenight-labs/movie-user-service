-- Migration: AddLastLoginToUser
-- Adiciona o campo last_login_at à tabela users

-- Adicionar coluna last_login_at
ALTER TABLE users 
ADD COLUMN last_login_at timestamp with time zone NULL;

-- Comentário da coluna
COMMENT ON COLUMN users.last_login_at IS 'Data e hora do último login do usuário';

-- Índice para melhorar performance em consultas por último login
CREATE INDEX CONCURRENTLY IF NOT EXISTS ix_users_last_login_at 
ON users (last_login_at) 
WHERE last_login_at IS NOT NULL;

-- Atualizar a versão da migration na tabela __EFMigrationsHistory
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250128000003_AddLastLoginToUser', '9.0.0');