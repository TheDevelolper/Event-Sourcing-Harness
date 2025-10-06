module.exports = {
  content: [
    "./libs/**/*.{js,ts,jsx,tsx}",
    "./apps/**/*.{js,ts,jsx,tsx}",
    "./apps/web/index.html",
  ],
  theme: {
    extend: {
      fontSize: {
        heading1: ["2.5rem", "3rem"],
        heading2: ["2rem", "2.5rem"],
        heading3: ["1.5rem", "2rem"],
      },
      fontWeight: {
        heading: "700",
      },
    },
  },
  plugins: [],
};
