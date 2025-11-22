// TODO: re-implement this later because, I had to remove tsjest. It wasn't keeping up-to-date with his own dependencies and so introduced vulnerabilities.

// /// <reference types="jest" />
// import axios from "axios";
// import MockAdapter from "axios-mock-adapter";
// import { RestaurantMenuService } from "./restaurant-menu.service";
// import { MenuItem } from "@ui-models";

// const mockData: Record<string, MenuItem[]> = {
//   pizza: [
//     {
//       id: 1,
//       name: "Margherita",
//       description: "Classic pizza",
//       price: 8.99,
//       image: "https://example.com/margherita.jpg",
//     },
//   ],
//   salad: [
//     {
//       id: 2,
//       name: "Caesar Salad",
//       description: "Fresh salad",
//       price: 7.5,
//       image: "https://example.com/caesar.jpg",
//     },
//   ],
// };

// describe("RestaurantMenuService", () => {
//   const mock = new MockAdapter(axios);
//   const menuUrl = "/menu.json";
//   let service: RestaurantMenuService;

//   beforeEach(() => {
//     mock.reset(); // reset mocks before each test
//     service = new RestaurantMenuService(menuUrl);
//   });

//   it("should fetch menu items successfully", async () => {
//     mock.onGet(menuUrl).reply(200, mockData);

//     const result = await service.fetchAllAsync();

//     expect(result).toEqual(mockData);
//     expect(await service.getAllAsync()).toEqual(mockData);
//   });
// });
