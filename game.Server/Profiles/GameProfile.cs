using AutoMapper;
using game.Server.Models;
using game.Server.DTOs;

namespace game.Server.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            // bank
            CreateMap<InventoryItem, BankInventoryDto>();
            CreateMap<ItemInstance, ItemInstanceDto>();
            CreateMap<Item, ItemDto>();

            //blueprint
            CreateMap<Blueprint, BlueprintDto>();
            CreateMap<Crafting, CraftingDto>();
            CreateMap<Item, CraftingItemDto>();

            //building
            CreateMap<Building, BuildingDto>();
            CreateMap<Floor, FloorDto>();
            CreateMap<FloorItem, FloorItemDto>();
            CreateMap<Chest, ChestDto>();
            CreateMap<Enemy, EnemyDto>();

            //mine
            CreateMap<MineBlock, MineBlockDto>();
            CreateMap<Block, MineBlockDetailsDto>();
            CreateMap<MineLayer, MineLayerDto>();
            CreateMap<FloorItem, MineItemDto>();

            //player
            CreateMap<Player, PlayerDto>();
            CreateMap<InventoryItem, InventoryItemDto>();

            //recipe
            CreateMap<Recipe, RecipeDto>();
            CreateMap<Ingredience, RecipeIngredienceDto>();
            CreateMap<Player, PlayerNameDto>();
            CreateMap<RecipeTime, LeaderboardDto>()
                .ForMember(dest => dest.Player, opt => opt.Ignore());

        }
    }
}