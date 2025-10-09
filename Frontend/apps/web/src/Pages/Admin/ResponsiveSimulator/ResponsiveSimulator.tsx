type Device = "sm" | "md" | "lg";

interface ResponsiveSimulatorProps {
  src: string;
  device?: Device;
}

const DEVICE_SIZES: Record<
  Device,
  { width: number; height: number; scale: number }
> = {
  sm: { width: 640, height: 812, scale: 0.4 },
  md: { width: 768, height: 1024, scale: 0.62 },
  lg: { width: 1280, height: 800, scale: 1 }, // desktop
};

export function ResponsiveSimulator({
  src,
  device = "sm",
}: ResponsiveSimulatorProps) {
  const size = DEVICE_SIZES[device];
  const scale = size.scale;
  const iframe = (
    <iframe
      key={src + device}
      src={src}
      width={size.width}
      style={{
        display: "block",
        border: "none",
        transform: `scale(${scale})`,
        transformOrigin: "top left",

        height: `${size.height / scale}px`,
      }}
      title={`Responsive Simulation - ${device}`}
    />
  );

  if (device === "sm") {
    return (
      <div className="relative mx-auto border-gray-400 dark:border-gray-400 bg-gray-400 border-[14px] rounded-[2rem] h-[600px] w-[300px] shadow-xl">
        <div className="w-[148px] h-[18px] bg-gray-400 top-0 rounded-b-[1rem] left-1/2 -translate-x-1/2 absolute z-[99]"></div>
        <div className="h-[46px] w-[3px] bg-gray-400 absolute -start-[17px] top-[124px] rounded-s-lg"></div>
        <div className="h-[46px] w-[3px] bg-gray-400 absolute -start-[17px] top-[178px] rounded-s-lg"></div>
        <div className="h-[64px] w-[3px] bg-gray-400 absolute -end-[17px] top-[142px] rounded-e-lg"></div>
        <div className="rounded-[1rem] overflow-hidden w-[272px] h-[572px] bg-white p-2">
          {iframe}
        </div>
      </div>
    );
  }

  if (device === "md") {
    return (
      <div className="relative mx-auto border-gray-400 dark:border-gray-400 bg-gray-400 border-[14px] rounded-[2rem] h-[454px] max-w-[341px] md:h-[682px] md:max-w-[512px]">
        <div className="h-[32px] w-[3px] bg-gray-400 dark:bg-gray-400 absolute -start-[17px] top-[72px] rounded-s-lg"></div>
        <div className="h-[46px] w-[3px] bg-gray-400 dark:bg-gray-400 absolute -start-[17px] top-[124px] rounded-s-lg"></div>
        <div className="h-[46px] w-[3px] bg-gray-400 dark:bg-gray-400 absolute -start-[17px] top-[178px] rounded-s-lg"></div>
        <div className="h-[64px] w-[3px] bg-gray-400 dark:bg-gray-400 absolute -end-[17px] top-[142px] rounded-e-lg"></div>
        <div className="rounded-[1rem] overflow-hidden h-[426px] md:h-[654px] bg-white ">
          {iframe}
        </div>
      </div>
    );
  }

  if (device === "lg") {
    return iframe;
  }
}
