import { useState } from "react";
import "./App.css";
import { RestaurantMenu } from "ui-components";
function App() {
  const [count, setCount] = useState(0);

  return (
    <div>
      <RestaurantMenu></RestaurantMenu>
    </div>
  );
}

export default App;
