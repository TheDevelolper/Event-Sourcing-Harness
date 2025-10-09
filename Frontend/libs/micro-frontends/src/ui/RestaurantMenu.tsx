import { useState, useEffect } from "react";
import { RestaurantMenu as UiRestaurantMenu } from "@ui-components";
import { RestaurantMenuService } from "../services/RestaurantMenu/restaurant-menu.service";
import { MenuItem } from "../../../ui-models/src";

export const RestaurantMenu: React.FC = () => {
  const [menu, setMenu] = useState<Record<string, MenuItem[]> | null>();

  const service: RestaurantMenuService = new RestaurantMenuService(
    "http://localhost:5173/sample.json"
  );

  useEffect(() => {
    service.fetchAllAsync().then(setMenu);
  }, []);

  if (!menu) return <div>Loading menu…</div>;

  return <UiRestaurantMenu items={menu} />;
};
