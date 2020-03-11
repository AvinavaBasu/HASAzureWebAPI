/**
 * Base abstract class to force client side data sources to have an id.
 */
export abstract class ZoomDataSource {
    /**
     * Id to set the data.
     */
    id: number | string;
}

export class ZoomSelectOption extends ZoomDataSource {
    id: string;
    name: string;
    description?: string;
    options?: ZoomSelectOption[];
}