export interface PageHeadingProps {
  title: string;
  children: React.ReactNode;
}

export function PageHeading(props: PageHeadingProps) {
  return (
    <div className="flex md:items-center md:justify-between mt-4 ">
      <div className="min-w-0 flex-1">
        <h2 className="text-2xl/7 font-bold text-gray-900 sm:truncate sm:text-3xl sm:tracking-tight">
          {props.title}
        </h2>
      </div>
      <div className="flex md:mt-0 md:ml-4">{props.children}</div>
    </div>
  );
}
