export interface DotNetObjectReference {
    invokeMethodAsync(methodName: string, ...parameters: any[]): Promise<any>;
}
