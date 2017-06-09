﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace MFaaP.MFWSClient
{
	/// <summary>
	/// Methods to create and modify objects.
	/// </summary>
	public class MFWSVaultObjectOperations
		: MFWSVaultOperationsBase
	{
		/// <summary>
		/// Creates a new <see cref="MFWSVaultObjectOperations"/> object.
		/// </summary>
		/// <param name="client">The client to interact with the server.</param>
		internal MFWSVaultObjectOperations(MFWSClientBase client)
			: base(client)
		{
		}

		#region Creating new objects

		/// <summary>
		/// Creates an object.
		/// </summary>
		/// <param name="objectTypeId">The type of the object.</param>
		/// <param name="creationInfo">The creation information for the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the created object.</returns>
		public async Task<ObjectVersion> CreateNewObjectAsync(int objectTypeId, ObjectCreationInfo creationInfo, CancellationToken token = default(CancellationToken))
		{

			// Sanity.
			if (null == creationInfo)
				throw new ArgumentNullException();
			if (objectTypeId < 0)
				throw new ArgumentException("The object type id cannot be less than zero");

			// Create the request.
			var request = new RestRequest($"/REST/objects/{objectTypeId}");
			request.AddJsonBody(creationInfo);

			// Make the request and get the response.
			var response = await this.MFWSClient.Post<ObjectVersion>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data;

		}

		/// <summary>
		/// Creates an object.
		/// </summary>
		/// <param name="objectTypeId">The type of the object.</param>
		/// <param name="creationInfo">The creation information for the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the created object.</returns>
		public ObjectVersion CreateNewObject(int objectTypeId, ObjectCreationInfo creationInfo, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.CreateNewObjectAsync(objectTypeId, creationInfo, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

		#region Checking in and out

		/// <summary>
		/// Sets an object checkout status.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="status">The checkout status.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public Task<ObjectVersion> SetCheckoutStatusAsync(int objectTypeId, int objectId, MFCheckOutStatus status, int? version = null,
			CancellationToken token = default(CancellationToken))
		{

			// Sanity.
			if (objectTypeId < 0)
				throw new ArgumentException("The object type id cannot be less than zero");
			if (objectId <= 0)
				throw new ArgumentException("The object id cannot be less than or equal to zero");

			// Use the other overload.
			return this.SetCheckoutStatusAsync(new ObjID()
			{
				ID = objectId,
				Type = objectTypeId
			}, status, version, token);
		}

		/// <summary>
		/// Sets an object checkout status.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="status">The checkout status.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public ObjectVersion SetCheckoutStatus(int objectTypeId, int objectId, MFCheckOutStatus status, int? version = null,
			CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.SetCheckoutStatusAsync(objectTypeId, objectId, status, version, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Sets an object checkout status.
		/// </summary>
		/// <param name="objId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="status">The checkout status.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public async Task<ObjectVersion> SetCheckoutStatusAsync(ObjID objId, MFCheckOutStatus status, int? version = null,
			CancellationToken token = default(CancellationToken))
		{

			// Sanity.
			if (null == objId)
				throw new ArgumentNullException(nameof(objId));

			// Create the request.
			var request = new RestRequest($"/REST/objects/{objId.Type}/{objId.ID}/{version?.ToString() ?? "latest"}/checkedout");
			request.AddJsonBody(new PrimitiveType<MFCheckOutStatus>() { Value = status });

			// Make the request and get the response.
			var response = await this.MFWSClient.Put<ObjectVersion>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data;
		}

		/// <summary>
		/// Sets an object checkout status.
		/// </summary>
		/// <param name="objId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="status">The checkout status.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public ObjectVersion SetCheckoutStatus(ObjID objId, MFCheckOutStatus status, int? version = null,
			CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.SetCheckoutStatusAsync(objId, status, version, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Sets an object checkout status.
		/// </summary>
		/// <param name="objVer">The Id and version of the object.</param>
		/// <param name="status">The checkout status.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public async Task<ObjectVersion> SetCheckoutStatusAsync(ObjVer objVer, MFCheckOutStatus status, CancellationToken token = default(CancellationToken))
		{

			// Sanity.
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));

			// Create the request.
			var request = new RestRequest($"/REST/objects/{objVer.Type}/{objVer.ID}/{objVer.Version}/checkedout");
			request.AddJsonBody(status);

			// Make the request and get the response.
			var response = await this.MFWSClient.Put<ObjectVersion>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data;
		}

		/// <summary>
		/// Sets an object checkout status.
		/// </summary>
		/// <param name="objVer">The Id and version of the object.</param>
		/// <param name="status">The checkout status.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public ObjectVersion SetCheckoutStatus(ObjVer objVer, MFCheckOutStatus status, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.SetCheckoutStatusAsync(objVer, status, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Retrieves an object' checkout status.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public Task<MFCheckOutStatus?> GetCheckoutStatusAsync(int objectTypeId, int objectId, int? version = null, CancellationToken token = default(CancellationToken))
		{

			// Sanity.
			if (objectTypeId < 0)
				throw new ArgumentException("The object type id cannot be less than zero");
			if (objectId <= 0)
				throw new ArgumentException("The object id cannot be less than or equal to zero");

			// Use the other overload.
			if (null == version)
			{
				return this.GetCheckoutStatusAsync(new ObjID()
				{
					ID = objectId,
					Type = objectTypeId
				}, token: token);
			}
			else
			{
				return this.GetCheckoutStatusAsync(new ObjVer()
				{
					ID = objectId,
					Type = objectTypeId,
					Version = version.Value
				}, token);
			}
		}

		/// <summary>
		/// Retrieves an object' checkout status.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public MFCheckOutStatus? GetCheckoutStatus(int objectTypeId, int objectId, int? version = null, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.GetCheckoutStatusAsync(objectTypeId, objectId, version, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Retrieves an object' checkout status.
		/// </summary>
		/// <param name="objId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public async Task<MFCheckOutStatus?> GetCheckoutStatusAsync(ObjID objId, int? version = null, CancellationToken token = default(CancellationToken))
		{

			// Sanity.
			if (null == objId)
				throw new ArgumentNullException(nameof(objId));

			// Create the request.
			var request = new RestRequest($"/REST/objects/{objId.Type}/{objId.ID}/{version?.ToString() ?? "latest"}/checkedout");

			// Make the request and get the response.
			var response = await this.MFWSClient.Get<PrimitiveType<MFCheckOutStatus>>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data?.Value;
		}

		/// <summary>
		/// Retrieves an object' checkout status.
		/// </summary>
		/// <param name="objId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public MFCheckOutStatus? GetCheckoutStatus(ObjID objId, int? version = null, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.GetCheckoutStatusAsync(objId, version, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Retrieves an object' checkout status.
		/// </summary>
		/// <param name="objVer">The Id and version of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public async Task<MFCheckOutStatus?> GetCheckoutStatusAsync(ObjVer objVer, CancellationToken token = default(CancellationToken))
		{

			// Sanity.
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));

			// Create the request.
			var request = new RestRequest($"/REST/objects/{objVer.Type}/{objVer.ID}/{objVer.Version}/checkedout");

			// Make the request and get the response.
			var response = await this.MFWSClient.Get<PrimitiveType<MFCheckOutStatus>>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data?.Value;
		}

		/// <summary>
		/// Retrieves an object' checkout status.
		/// </summary>
		/// <param name="objVer">The Id and version of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public MFCheckOutStatus? GetCheckoutStatus(ObjVer objVer, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.GetCheckoutStatusAsync(objVer, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Checks out an object.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public Task<ObjectVersion> CheckOutAsync(int objectTypeId, int objectId, int? version = null, CancellationToken token = default(CancellationToken))
		{
			return this.SetCheckoutStatusAsync(objectTypeId, objectId, MFCheckOutStatus.CheckedOutToMe, version, token);
		}

		/// <summary>
		/// Checks out an object.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public ObjectVersion CheckOut(int objectTypeId, int objectId, int? version = null, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.CheckOutAsync(objectTypeId, objectId, version, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Checks in an object.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public Task<ObjectVersion> CheckInAsync(int objectTypeId, int objectId, int? version = null, CancellationToken token = default(CancellationToken))
		{
			return this.SetCheckoutStatusAsync(objectTypeId, objectId, MFCheckOutStatus.CheckedIn, version, token);
		}

		/// <summary>
		/// Checks in an object.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="version">The version (or null for latest).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public ObjectVersion CheckIn(int objectTypeId, int objectId, int? version = null, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.CheckInAsync(objectTypeId, objectId, version, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

		#region Deleted status

		/// <summary>
		/// Retrieves an object's deleted status.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public Task<bool?> GetDeletedStatusAsync(int objectTypeId, int objectId, CancellationToken token = default(CancellationToken))
		{

			// Sanity.
			if (objectTypeId < 0)
				throw new ArgumentException("The object type id cannot be less than zero");
			if (objectId <= 0)
				throw new ArgumentException("The object id cannot be less than or equal to zero");

			// Use the other overload.
			return this.GetDeletedStatusAsync(new ObjID()
			{
				ID = objectId,
				Type = objectTypeId
			}, token);
		}

		/// <summary>
		/// Retrieves an object's deleted status.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public bool? GetDeletedStatus(int objectTypeId, int objectId, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.GetDeletedStatusAsync(objectTypeId, objectId, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Retrieves an object's deleted status.
		/// </summary>
		/// <param name="objId">The Id of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public async Task<bool?> GetDeletedStatusAsync(ObjID objId, CancellationToken token = default(CancellationToken))
		{

			// Sanity.
			if (null == objId)
				throw new ArgumentNullException(nameof(objId));

			// Create the request.
			var request = new RestRequest($"/REST/objects/{objId.Type}/{objId.ID}/deleted");

			// Make the request and get the response.
			var response = await this.MFWSClient.Get<PrimitiveType<bool>>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data?.Value;
		}

		/// <summary>
		/// Retrieves an object's deleted status.
		/// </summary>
		/// <param name="objId">The Id of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A representation of the checked-in object version/</returns>
		public bool? GetDeletedStatus(ObjID objId, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.GetDeletedStatusAsync(objId, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

		#region History

		/// <summary>
		/// Retrieves the properties of multiple objects.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A collection of <see cref="ObjectVersion"/>s representing the object history.</returns>
		/// <remarks>Note that not all versions may be shown: http://www.m-files.com/mfws/resources/objects/type/objectid/history.html</remarks>
		public Task<List<ObjectVersion>> GetHistoryAsync(int objectTypeId, int objectId, CancellationToken token = default(CancellationToken))
		{
			return this.GetHistoryAsync(new ObjID()
			{
				Type = objectTypeId,
				ID = objectId
			}, token);

		}

		/// <summary>
		/// Retrieves the properties of multiple objects.
		/// </summary>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A collection of <see cref="ObjectVersion"/>s representing the object history.</returns>
		/// <remarks>Note that not all versions may be shown: http://www.m-files.com/mfws/resources/objects/type/objectid/history.html</remarks>
		public List<ObjectVersion> GetHistory(int objectTypeId, int objectId, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.GetHistoryAsync(objectTypeId, objectId, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Retrieves the properties of multiple objects.
		/// </summary>
		/// <param name="objID">The object to retrieve the history from.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A collection of <see cref="ObjectVersion"/>s representing the object history.</returns>
		/// <remarks>Note that not all versions may be shown: http://www.m-files.com/mfws/resources/objects/type/objectid/history.html</remarks>
		public async Task<List<ObjectVersion>> GetHistoryAsync(ObjID objID, CancellationToken token = default(CancellationToken))
		{
			// Sanity.
			if (null == objID)
				throw new ArgumentNullException(nameof(ObjID));

			// Create the request.
			var request = new RestRequest($"/REST/objects/{objID.Type}/{objID.ID}/history");

			// Make the request and get the response.
			var response = await this.MFWSClient.Get<List<ObjectVersion>>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data;

		}

		/// <summary>
		/// Retrieves the properties of multiple objects.
		/// </summary>
		/// <param name="objID">The object to retrieve the history from.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>A collection of <see cref="ObjectVersion"/>s representing the object history.</returns>
		/// <remarks>Note that not all versions may be shown: http://www.m-files.com/mfws/resources/objects/type/objectid/history.html</remarks>
		public List<ObjectVersion> GetHistory(ObjID objID, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.GetHistoryAsync(objID, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();

		}

		#endregion

		#region Favourites

		/// <summary>
		/// Adds the supplied item to the favorites.
		/// </summary>
		/// <param name="objId">The <see cref="ObjID"/> of the item to add to the favourites.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The item that was added.</returns>
		public async Task<ExtendedObjectVersion> AddToFavoritesAsync(ObjID objId, CancellationToken token = default(CancellationToken))
		{
			// Sanity.
			if (null == objId)
				throw new ArgumentNullException(nameof(objId));

			// Create the request.
			var request = new RestRequest("/REST/favorites");
			request.AddJsonBody(objId);

			// Make the request and get the response.
			var response = await this.MFWSClient.Post<ExtendedObjectVersion>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data;
		}

		/// <summary>
		/// Adds the supplied item to the favorites.
		/// </summary>
		/// <param name="objId">The <see cref="ObjID"/> of the item to add to the favourites.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The item that was added.</returns>
		public ExtendedObjectVersion AddToFavorites(ObjID objId, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.AddToFavoritesAsync(objId, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Adds the supplied item to the favorites.
		/// </summary>
		/// <param name="objectTypeId">The id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The item that was added.</returns>
		public Task<ExtendedObjectVersion> AddToFavoritesAsync(int objectTypeId, int objectId, CancellationToken token = default(CancellationToken))
		{
			return this.AddToFavoritesAsync(new ObjID()
			{
				ID = objectId,
				Type = objectTypeId
			}, token);
		}

		/// <summary>
		/// Adds the supplied item to the favorites.
		/// </summary>
		/// <param name="objectTypeId">The id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The item that was added.</returns>
		public ExtendedObjectVersion AddToFavorites(int objectTypeId, int objectId, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.AddToFavoritesAsync(objectTypeId, objectId, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Removes the supplied item to the favorites.
		/// </summary>
		/// <param name="objId">The <see cref="ObjID"/> of the item to remove from the favourites.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The item that was removed.</returns>
		public async Task<ExtendedObjectVersion> RemoveFromFavoritesAsync(ObjID objId, CancellationToken token = default(CancellationToken))
		{
			// Sanity.
			if (null == objId)
				throw new ArgumentNullException(nameof(objId));

			// Create the request.
			var request = new RestRequest($"/REST/favorites/{objId.Type}/{objId.ID}");

			// Make the request and get the response.
			var response = await this.MFWSClient.Delete<ExtendedObjectVersion>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data;
		}

		/// <summary>
		/// Removes the supplied item to the favorites.
		/// </summary>
		/// <param name="objId">The <see cref="ObjID"/> of the item to remove from the favourites.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The item that was removed.</returns>
		public ExtendedObjectVersion RemoveFromFavorites(ObjID objId, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.RemoveFromFavoritesAsync(objId, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Removes the supplied item to the favorites.
		/// </summary>
		/// <param name="objectTypeId">The id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The item that was removed.</returns>
		public Task<ExtendedObjectVersion> RemoveFromFavoritesAsync(int objectTypeId, int objectId, CancellationToken token = default(CancellationToken))
		{
			return this.RemoveFromFavoritesAsync(new ObjID()
			{
				ID = objectId,
				Type = objectTypeId
			}, token);
		}

		/// <summary>
		/// Removes the supplied item to the favorites.
		/// </summary>
		/// <param name="objectTypeId">The id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The item that was removed.</returns>
		public ExtendedObjectVersion RemoveFromFavorites(int objectTypeId, int objectId, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.RemoveFromFavoritesAsync(objectTypeId, objectId, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

	}
	
}