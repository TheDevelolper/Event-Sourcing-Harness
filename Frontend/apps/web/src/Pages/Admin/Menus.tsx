import { useState } from "react";
import { ResponsiveSimulator } from "./ResponsiveSimulator/ResponsiveSimulator";
import { PageHeading } from "./PageHeading/PageHeading";

export default function Menus() {
  const [device, setDevice] = useState<"sm" | "md" | "lg">("sm");

  return (
    <div className="w-[80%] mx-auto my-4">
      <PageHeading title="Menus">
        <button
          type="button"
          className="inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-xs inset-ring inset-ring-gray-300 hover:bg-gray-50"
        >
          Edit
        </button>

        <button
          type="button"
          className="ml-3 inline-flex items-center rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-xs hover:bg-indigo-700 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
        >
          Publish
        </button>
      </PageHeading>

      <div className="max-w-[1250px] w-fit mx-auto mb-8 p-8">
        {/* Device toggle buttons */}
        <div className="flex justify-center gap-3 mt-6">
          {(
            [
              { name: "Mobile", size: "sm" },
              { name: "Tablet", size: "md" },
              { name: "Desktop", size: "lg" },
            ] as const
          ).map((deviceSize) => (
            <button
              key={deviceSize.name}
              onClick={() => setDevice(deviceSize.size)}
              className={`px-4 py-2 rounded-md text-sm font-medium transition ${
                device === deviceSize.size
                  ? "bg-indigo-600 text-white shadow"
                  : "bg-gray-100 text-gray-700 hover:bg-gray-200"
              }`}
            >
              {deviceSize.name}
            </button>
          ))}
        </div>
        <div className="mt-8">
          <ResponsiveSimulator src="/" device={device}></ResponsiveSimulator>
        </div>
      </div>
    </div>
  );
}
