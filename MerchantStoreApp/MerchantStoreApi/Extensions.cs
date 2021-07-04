using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MerchantStoreApi.Dtos;
using MerchantStoreApi.Entities;

namespace MerchantStoreApi
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate
            };
        }
    }
}
