﻿using CardapioOnlineAPI.Dto;
using CardapioOnlineAPI.Entities;
using CardapioOnlineAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CardapioOnlineAPI.Services
{
    public class MenuService
    {
        private readonly MenuRepository _repository;

        public MenuService(MenuRepository repository)
        {
            _repository = repository;
        }

        public List<MenuItem> GetAllMenuItems()
        {
            return _repository.GetAllMenuItems();
        }

        public void AddMenuItem(CreateRequest request)
        {
            var menuItem = new MenuItem()
            {
                Description = request.Description,
                Name = request.Name,
                Price = request.Price,
            };

            _repository.AddMenuItem(menuItem);
        }

        public MenuItem GetMenuItemById(int id)
        {
            var retorno = _repository.GetMenuItemById(id);

            return retorno; 
        }

        public void UpdateMenuItem(int id, UpdateRequest request)
        {
            var exist = GetMenuItemById(id);

            MenuItem menuItem = UpdateRequest.FromUpdateRequest(request);

            if (exist != null)
            {
                _repository.UpdateMenuItem(menuItem);
            }
        }

        public void DeleteMenuItem(int id)
        {
            _repository.DeleteMenuItem(id);
        }

        public async Task<MenuItem> UploadImage(int id, IFormFile file)
        {
            var menuItem = GetMenuItemById(id);

            if (menuItem == null)

                throw new Exception("MenuItem não localizado");

            if (file == null)

                throw new Exception("Arquivo está vazio");


            string uploadsFolder = Path.Combine(@"C:\Cardapio\upload");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            menuItem.ImageUrl = filePath;
            UpdateMenuItem(id, UpdateRequest.FromMenuItem(menuItem));


            return menuItem;
        }
    }
}
