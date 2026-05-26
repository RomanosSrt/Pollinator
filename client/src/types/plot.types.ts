export interface Geometry {
  type: string;
  coordinates: number[][][]; 
}
 
export interface PlotDto {
  plotId: string; 
  polygon: Geometry;
  area: number;
  cropTypes?: CropType[]; 
  isClaimed: boolean;
  farmerId?: number | null; 
}

export const  CropType = {
  THYME: "THYME",
  PINE: "PINE",
  FIR : "FIR",        // Ελάτης
  CHESTNUT : "CHESTNUT",
  ORANGE : "ORANGE",
  BLOSSOM : "BLOSSOM" // Ανθόμελο
} as const;

export type CropType = typeof CropType[keyof typeof CropType];