// jest.config.cjs
/** @type {import('ts-jest').JestConfigWithTsJest} */
module.exports = {
  preset: "ts-jest/presets/default-esm", // ESM preset for ts-jest
  testEnvironment: "node",
  extensionsToTreatAsEsm: [".ts"],
};
