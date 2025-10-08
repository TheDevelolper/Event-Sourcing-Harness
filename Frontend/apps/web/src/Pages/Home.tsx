import { Card } from "@ui-components";

export default function Home() {
  return (
    <div className="w-[100vw]">
      <Card
        className="m-auto w-fit text-center px-8 py-16"
        shadow
        variant="primary"
      >
        <h1 className="text-3xl font-bold underline mb-4">Saas Factory UI</h1>
        <p className="text-lg mb-2">
          A page with more instructions will be coming here soon. Stay tuned!
        </p>
        <p className="text-sm">
          You can visit the admin dashboard by heading over to /admin
        </p>
      </Card>
    </div>
  );
}
