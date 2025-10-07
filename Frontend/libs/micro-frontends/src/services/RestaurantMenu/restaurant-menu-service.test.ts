/// <reference types="jest" />
import axios from "axios";
import MockAdapter from "axios-mock-adapter";
import { RestaurantMenuService } from "./restaurant-menu.service";
import { MenuItem } from "../../../../ui-models/src";

describe("RestaurantMenuService", () => {
  const mock = new MockAdapter(axios);
  const menuUrl = "/menu.json";
  let service: RestaurantMenuService;

  beforeEach(() => {
    service = new RestaurantMenuService(menuUrl);
    mock.reset(); // reset mocks before each test
  });

  it("should return empty menuItems initially", async () => {
    expect(await service.getAllAsync()).toEqual({});
  });

  it("should fetch menu items successfully", async () => {
    const mockData: Record<string, MenuItem[]> = {
      pizza: [
        {
          id: 1,
          name: "Margherita",
          description: "Classic pizza",
          price: 8.99,
          image: "https://example.com/margherita.jpg",
        },
      ],
      salad: [
        {
          id: 2,
          name: "Caesar Salad",
          description: "Fresh salad",
          price: 7.5,
          image: "https://example.com/caesar.jpg",
        },
      ],
    };

    mock.onGet(menuUrl).reply(200, mockData);

    const result = await service.fetchAll();

    expect(result).toEqual(mockData);
    expect(service.getAll()).toEqual(mockData);
  });

  //   it("should handle fetch errors gracefully", async () => {
  //     mock.onGet(menuUrl).networkError();

  //     const result = await service.fetchAll();

  //     expect(result).toEqual({});
  //     expect(service.getAll()).toEqual({});
  //   });
});
