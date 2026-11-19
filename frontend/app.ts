type FeatureId = "rampa" | "banheiro" | "piso" | "elevador" | "sinalizacao";
type FilterId = FeatureId | "all";
type PlaceKind = "culture" | "cafe" | "library" | "park";

interface Place {
  id: string;
  name: string;
  category: string;
  address: string;
  rating: number;
  reviewCount: number;
  kind: PlaceKind;
  features: FeatureId[];
  mapPosition: { x: number; y: number };
}

const featureInfo: Record<FeatureId, { label: string; shortLabel: string }> = {
  rampa: { label: "Entrada sem degraus", shortLabel: "Entrada acessível" },
  banheiro: { label: "Banheiro acessível", shortLabel: "Banheiro adaptado" },
  piso: { label: "Piso tátil", shortLabel: "Piso tátil" },
  elevador: { label: "Elevador", shortLabel: "Elevador" },
  sinalizacao: { label: "Sinalização acessível", shortLabel: "Sinalização acessível" },
};

// Conteúdo fictício para demonstrar a interface; não representa estabelecimentos verificados.
const places: Place[] = [
  {
    id: "centro-vagalume",
    name: "Centro Cultural Vaga-Lume",
    category: "Cultura e lazer",
    address: "Rua das Acácias, 214 · Centro",
    rating: 4.9,
    reviewCount: 24,
    kind: "culture",
    features: ["rampa", "banheiro", "piso", "sinalizacao"],
    mapPosition: { x: 31, y: 34 },
  },
  {
    id: "cafe-entrelinhas",
    name: "Café Entrelinhas",
    category: "Café e alimentação",
    address: "Travessa do Ipê, 58 · Centro",
    rating: 4.7,
    reviewCount: 18,
    kind: "cafe",
    features: ["rampa", "banheiro", "sinalizacao"],
    mapPosition: { x: 66, y: 42 },
  },
  {
    id: "biblioteca-parque",
    name: "Biblioteca do Parque",
    category: "Biblioteca e estudo",
    address: "Avenida do Encontro, 900 · Jardim",
    rating: 4.8,
    reviewCount: 31,
    kind: "library",
    features: ["rampa", "banheiro", "piso", "elevador", "sinalizacao"],
    mapPosition: { x: 49, y: 67 },
  },
  {
    id: "parque-bem-te-vi",
    name: "Parque Bem-te-vi",
    category: "Parque e área verde",
    address: "Alameda das Flores, 120 · Vila Nova",
    rating: 4.6,
    reviewCount: 16,
    kind: "park",
    features: ["rampa", "banheiro", "piso"],
    mapPosition: { x: 78, y: 75 },
  },
];

const placeArt: Record<PlaceKind, string> = {
  culture: '<svg viewBox="0 0 48 48" aria-hidden="true"><path d="M8 18.5 24 9l16 9.5M11 20v17m9-17v17m8-17v17m9-17v17M7 38h34M6 18h36" /><path d="M16 13.8h16M18 9.5l1.7-4h8.6l1.7 4" /></svg>',
  cafe: '<svg viewBox="0 0 48 48" aria-hidden="true"><path d="M11 19h25l-2.4 18H13.4L11 19Zm25 4h3.2a4.3 4.3 0 0 1 0 8.6h-4.3M17 13c-2.2-2.3 2.6-3.5.4-6m8.4 6c-2.2-2.3 2.6-3.5.4-6" /><path d="M16 24h12m-10 5h10" /></svg>',
  library: '<svg viewBox="0 0 48 48" aria-hidden="true"><path d="M8 39V13l16-7 16 7v26M6 39h36M14 19h8v8h-8zm12 0h8v8h-8zM14 31h8v8h-8zm12 0h8v8h-8z" /><path d="M22 39v-5h4v5" /></svg>',
  park: '<svg viewBox="0 0 48 48" aria-hidden="true"><path d="M24 40V22m0 7-9-7m9 3 8-7" /><path d="M12 20c-5.8-4.6-2.9-11.5 3.3-11.7.4-4 7.4-5.4 9.3.2 5-3.2 10.6 1.8 7.5 6.2 6 .3 7.1 7.8 1.8 10.1-2.1 4.6-9.1 4.7-11.7.8-3.7 3.6-9.9 1.3-10.2-5.6Z" /><path d="M17 40h14" /></svg>',
};

const requiredElement = <T extends HTMLElement>(id: string): T => {
  const element = document.getElementById(id);
  if (!element) throw new Error(`Elemento obrigatório não encontrado: #${id}`);
  return element as T;
};

const searchForm = requiredElement<HTMLFormElement>("search-form");
const searchInput = requiredElement<HTMLInputElement>("search-input");
const sortSelect = requiredElement<HTMLSelectElement>("sort-select");
const resultsList = requiredElement<HTMLDivElement>("results-list");
const resultsCount = requiredElement<HTMLParagraphElement>("results-count");
const emptyState = requiredElement<HTMLDivElement>("empty-state");
const mapMarkers = requiredElement<HTMLDivElement>("map-markers");
const filterButtons = Array.from(document.querySelectorAll<HTMLButtonElement>(".filter-chip[data-filter]"));
const clearFiltersButton = requiredElement<HTMLButtonElement>("clear-filters");
const emptyResetButton = requiredElement<HTMLButtonElement>("empty-reset");
const textSizeToggle = requiredElement<HTMLButtonElement>("text-size-toggle");
const locationButton = requiredElement<HTMLButtonElement>("location-button");
const accountButton = requiredElement<HTMLButtonElement>("account-button");
const contributeButton = requiredElement<HTMLButtonElement>("contribute-button");
const dialogLayer = requiredElement<HTMLDivElement>("dialog-layer");
const dialog = requiredElement<HTMLElement>("dialog");
const dialogContent = requiredElement<HTMLDivElement>("dialog-content");
const dialogClose = requiredElement<HTMLButtonElement>("dialog-close");
const dialogScrim = requiredElement<HTMLButtonElement>("dialog-scrim");
const toast = requiredElement<HTMLDivElement>("toast");

let activeFilter: FilterId = "all";
let toastTimeout = 0;
let previousFocus: HTMLElement | null = null;

const escapeHtml = (value: string): string =>
  value.replace(/[&<>"']/g, (character) => {
    const entities: Record<string, string> = {
      "&": "&amp;",
      "<": "&lt;",
      ">": "&gt;",
      '"': "&quot;",
      "'": "&#39;",
    };
    return entities[character] ?? character;
  });

const starIcon = '<svg viewBox="0 0 16 16" aria-hidden="true"><path d="m8 1.4 2 4.1 4.5.7-3.2 3.2.8 4.5L8 11.8l-4.1 2.1.8-4.5L1.5 6.2 6 5.5 8 1.4Z" /></svg>';
const pinIcon = '<svg viewBox="0 0 16 16" aria-hidden="true"><path d="M13 6.7c0 3.5-5 7.4-5 7.4S3 10.2 3 6.7a5 5 0 1 1 10 0Z" /><circle cx="8" cy="6.6" r="1.5" /></svg>';
const markerIcon = '<svg viewBox="0 0 16 16" aria-hidden="true"><path d="M8 14s5-3.9 5-7.4a5 5 0 1 0-10 0C3 10.1 8 14 8 14Z" /><circle cx="8" cy="6.5" r="1.7" /></svg>';

const renderFeatures = (place: Place): string => {
  const labels = place.features
    .slice(0, 3)
    .map((featureId) => `<span class="accessibility-tag"><span aria-hidden="true">✓</span>${escapeHtml(featureInfo[featureId].shortLabel)}</span>`)
    .join("");
  const extraCount = Math.max(0, place.features.length - 3);
  const extra = extraCount > 0 ? `<span class="accessibility-tag">+${extraCount}</span>` : "";
  return `${labels}${extra}`;
};

const renderPlaceCard = (place: Place): string => `
  <article class="place-card" data-place-card="${escapeHtml(place.id)}">
    <div class="place-art" data-kind="${place.kind}" aria-hidden="true">${placeArt[place.kind]}</div>
    <div class="place-info">
      <p class="place-category">${escapeHtml(place.category)}</p>
      <div class="place-title-row">
        <h3 class="place-title">${escapeHtml(place.name)}</h3>
        <span class="community-tag">Comunidade</span>
      </div>
      <p class="place-address">${pinIcon}<span>${escapeHtml(place.address)}</span></p>
      <div class="place-meta">
        <span class="rating" aria-label="Nota ${place.rating.toFixed(1)} de 5">${starIcon}<span>${place.rating.toFixed(1)}</span></span>
        <span class="meta-divider" aria-hidden="true"></span>
        <span class="review-count">${place.reviewCount} relatos</span>
      </div>
      <div class="accessibility-tags" aria-label="Recursos listados na demonstração">${renderFeatures(place)}</div>
    </div>
    <button class="place-detail-button" type="button" data-open-place="${escapeHtml(place.id)}" aria-label="Ver detalhes de ${escapeHtml(place.name)}">
      <svg viewBox="0 0 20 20" aria-hidden="true"><path d="M7.5 4.5 13 10l-5.5 5.5M4 10h9" /></svg>
    </button>
  </article>`;

const getFilteredPlaces = (): Place[] => {
  const query = searchInput.value.trim().toLocaleLowerCase("pt-BR");
  const filtered = places.filter((place) => {
    const searchableText = [
      place.name,
      place.category,
      place.address,
      ...place.features.map((featureId) => featureInfo[featureId].label),
    ].join(" ").toLocaleLowerCase("pt-BR");
    const matchesQuery = query.length === 0 || searchableText.includes(query);
    const matchesFeature = activeFilter === "all" || place.features.includes(activeFilter);
    return matchesQuery && matchesFeature;
  });

  if (sortSelect.value === "rating") return filtered.sort((first, second) => second.rating - first.rating);
  if (sortSelect.value === "name") return filtered.sort((first, second) => first.name.localeCompare(second.name, "pt-BR"));
  return filtered;
};

const renderMapMarkers = (visiblePlaces: Place[]): void => {
  mapMarkers.innerHTML = visiblePlaces.map((place) => `
    <button class="map-marker" type="button" data-map-place="${escapeHtml(place.id)}"
      style="left:${place.mapPosition.x}%;top:${place.mapPosition.y}%"
      aria-label="Ver detalhes de ${escapeHtml(place.name)}">${markerIcon}</button>`).join("");
};

const renderResults = (): void => {
  const visiblePlaces = getFilteredPlaces();
  resultsList.innerHTML = visiblePlaces.map(renderPlaceCard).join("");
  resultsList.setAttribute("aria-busy", "false");
  emptyState.hidden = visiblePlaces.length > 0;
  resultsList.hidden = visiblePlaces.length === 0;
  const resultNoun = visiblePlaces.length === 1 ? "espaço encontrado" : "espaços encontrados";
  resultsCount.textContent = `${visiblePlaces.length} ${resultNoun}`;
  renderMapMarkers(visiblePlaces);
};

const showToast = (message: string): void => {
  window.clearTimeout(toastTimeout);
  toast.textContent = message;
  toast.classList.add("is-visible");
  toastTimeout = window.setTimeout(() => toast.classList.remove("is-visible"), 3400);
};

const openDialog = (content: string, trigger?: HTMLElement | null): void => {
  previousFocus = trigger ?? (document.activeElement instanceof HTMLElement ? document.activeElement : null);
  dialogContent.innerHTML = content;
  dialogLayer.hidden = false;
  document.body.style.overflow = "hidden";
  dialog.focus();
};

const closeDialog = (): void => {
  dialogLayer.hidden = true;
  document.body.style.overflow = "";
  dialogContent.replaceChildren();
  previousFocus?.focus();
  previousFocus = null;
};

const showAccountDialog = (forContribution = false): void => {
  const title = forContribution ? "Sua experiência pode abrir caminhos." : "Faça parte de uma cidade mais acessível.";
  const description = forContribution
    ? "Avaliações, informações e relatos ajudam outras pessoas a planejar uma visita com mais autonomia."
    : "A plataforma permitirá que pessoas cadastradas avaliem espaços, compartilhem informações e acompanhem seus relatos.";
  const content = `
    <p class="dialog-eyebrow">ACESSO À COMUNIDADE</p>
    <h2 id="dialog-title">${title}</h2>
    <p class="dialog-description">${description}</p>
    <div class="dialog-note">Esta é uma demonstração visual: cadastro, autenticação e envio de informações ainda não estão conectados ao sistema. Nenhum dado pessoal será solicitado ou enviado.</div>
    <div class="dialog-actions">
      <button class="button button-primary" type="button" data-close-dialog>Continuar explorando</button>
    </div>`;
  openDialog(content);
};

const showPlaceDetails = (placeId: string, trigger?: HTMLElement | null): void => {
  const place = places.find((item) => item.id === placeId);
  if (!place) return;
  const features = place.features.map((featureId) => `<li>${escapeHtml(featureInfo[featureId].label)}</li>`).join("");
  const content = `
    <p class="dialog-eyebrow">${escapeHtml(place.category.toLocaleUpperCase("pt-BR"))} · RELATO DA COMUNIDADE</p>
    <h2 id="dialog-title">${escapeHtml(place.name)}</h2>
    <p class="dialog-place-address">${pinIcon}<span>${escapeHtml(place.address)}</span></p>
    <div class="dialog-rating-row">
      <span class="dialog-rating-score">${place.rating.toFixed(1)}</span>
      <span class="dialog-rating-stars" aria-label="estrelas">★★★★★</span>
      <span class="dialog-rating-meta">${place.reviewCount} relatos · demonstração</span>
    </div>
    <h3 class="dialog-section-title">Recursos de acessibilidade listados</h3>
    <ul class="dialog-feature-list">${features}</ul>
    <p class="dialog-note">Dados ilustrativos para apresentar a interface. Confirme diretamente com o estabelecimento; estas informações não são uma certificação técnica.</p>
    <div class="dialog-actions">
      <button class="button button-primary" type="button" data-continue-account>Quero compartilhar uma experiência</button>
      <button class="button button-outline" type="button" data-close-dialog>Fechar</button>
    </div>`;
  openDialog(content, trigger);
};

const resetFilters = (): void => {
  searchInput.value = "";
  activeFilter = "all";
  sortSelect.value = "recommended";
  filterButtons.forEach((button) => {
    const selected = button.dataset.filter === "all";
    button.classList.toggle("is-active", selected);
    button.setAttribute("aria-pressed", String(selected));
  });
  renderResults();
};

searchForm.addEventListener("submit", (event: SubmitEvent) => {
  event.preventDefault();
  renderResults();
});

searchInput.addEventListener("input", renderResults);
sortSelect.addEventListener("change", renderResults);

filterButtons.forEach((button) => {
  button.addEventListener("click", () => {
    const selectedFilter = button.dataset.filter as FilterId | undefined;
    if (!selectedFilter) return;
    activeFilter = selectedFilter;
    filterButtons.forEach((filterButton) => {
      const selected = filterButton === button;
      filterButton.classList.toggle("is-active", selected);
      filterButton.setAttribute("aria-pressed", String(selected));
    });
    renderResults();
  });
});

clearFiltersButton.addEventListener("click", resetFilters);
emptyResetButton.addEventListener("click", resetFilters);

resultsList.addEventListener("click", (event: MouseEvent) => {
  const target = event.target;
  if (!(target instanceof Element)) return;
  const button = target.closest<HTMLButtonElement>("[data-open-place]");
  const placeId = button?.dataset.openPlace;
  if (placeId) showPlaceDetails(placeId, button);
});

mapMarkers.addEventListener("click", (event: MouseEvent) => {
  const target = event.target;
  if (!(target instanceof Element)) return;
  const button = target.closest<HTMLButtonElement>("[data-map-place]");
  const placeId = button?.dataset.mapPlace;
  if (placeId) showPlaceDetails(placeId, button);
});

textSizeToggle.addEventListener("click", () => {
  const isLarge = document.body.classList.toggle("large-text");
  textSizeToggle.setAttribute("aria-pressed", String(isLarge));
  textSizeToggle.setAttribute("aria-label", isLarge ? "Restaurar tamanho padrão do texto" : "Aumentar o tamanho do texto");
  showToast(isLarge ? "Texto ampliado." : "Tamanho de texto padrão restaurado.");
});

locationButton.addEventListener("click", () => {
  showToast("A localização ainda não está conectada nesta demonstração.");
});

accountButton.addEventListener("click", () => showAccountDialog(false));
contributeButton.addEventListener("click", () => showAccountDialog(true));
dialogClose.addEventListener("click", closeDialog);
dialogScrim.addEventListener("click", closeDialog);

dialogContent.addEventListener("click", (event: MouseEvent) => {
  const target = event.target;
  if (!(target instanceof Element)) return;
  if (target.closest("[data-close-dialog]")) closeDialog();
  if (target.closest("[data-continue-account]")) showAccountDialog(true);
});

document.addEventListener("keydown", (event: KeyboardEvent) => {
  if (dialogLayer.hidden) return;
  if (event.key === "Escape") {
    closeDialog();
    return;
  }
  if (event.key !== "Tab") return;
  const focusable = Array.from(dialog.querySelectorAll<HTMLElement>(
    'button:not([disabled]), a[href], input:not([disabled]), select:not([disabled]), textarea:not([disabled]), [tabindex]:not([tabindex="-1"])',
  ));
  if (focusable.length === 0) {
    event.preventDefault();
    dialog.focus();
    return;
  }
  const first = focusable[0];
  const last = focusable[focusable.length - 1];
  if (event.shiftKey && document.activeElement === first) {
    event.preventDefault();
    last.focus();
  } else if (!event.shiftKey && document.activeElement === last) {
    event.preventDefault();
    first.focus();
  }
});

renderResults();
