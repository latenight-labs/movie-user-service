-- Script para inserir dados de exemplo na tabela users
-- Execute após aplicar as migrations

INSERT INTO users (name, username, email, phone, street, city, state, zip_code, country, created_at, is_active) VALUES
('John Doe', 'john_doe', 'john.doe@email.com', '+1234567890', '123 Main St', 'New York', 'NY', '10001', 'USA', NOW(), true),
('Jane Smith', 'jane_smith', 'jane.smith@email.com', '+1987654321', '456 Oak Ave', 'Los Angeles', 'CA', '90210', 'USA', NOW(), true),
('Carlos Silva', 'carlos_silva', 'carlos.silva@email.com', '+5511999888777', 'Rua das Flores, 789', 'São Paulo', 'SP', '01234-567', 'Brazil', NOW(), true),
('Maria Santos', 'maria_santos', 'maria.santos@email.com', '+5521888777666', 'Av. Copacabana, 1000', 'Rio de Janeiro', 'RJ', '22070-001', 'Brazil', NOW(), true),
('Pierre Martin', 'pierre_martin', 'pierre.martin@email.com', '+33123456789', '10 Rue de la Paix', 'Paris', 'Île-de-France', '75001', 'France', NOW(), true);