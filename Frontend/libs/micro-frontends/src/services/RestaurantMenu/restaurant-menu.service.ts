import axios from "axios";
import { MenuItem } from "../../../../ui-models/src";

export class RestaurantMenuService {
  constructor(private readonly menuUrl: string) {}

  private menuItems: Record<string, MenuItem[]> | null = null;

  public async getAllAsync(): Promise<Record<string, MenuItem[]>> {
    return this.menuItems ?? (await this.fetchAllAsync());
  }

  public async fetchAllAsync(): Promise<Record<string, MenuItem[]>> {
    try {
      const response = await axios.get<Record<string, MenuItem[]> | string>(
        this.menuUrl
      );

      this.menuItems =
        typeof response.data === "string"
          ? JSON.parse(response.data)
          : response.data;

  
      return this.menuItems as Record<string, MenuItem[]>;
    } catch (error) {
      console.error("Error fetching menu items:", error);
      return {};
    }
  }
}
