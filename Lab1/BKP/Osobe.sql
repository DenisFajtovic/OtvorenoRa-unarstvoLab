--
-- PostgreSQL database dump
--

-- Dumped from database version 15.0
-- Dumped by pg_dump version 15.0

-- Started on 2022-10-31 12:23:02

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 214 (class 1259 OID 16416)
-- Name: Osobe; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Osobe" (
    "OIB" character varying(50) NOT NULL,
    "Ime" character varying(50),
    "Prezime" character varying(50)
);


ALTER TABLE public."Osobe" OWNER TO postgres;

--
-- TOC entry 3318 (class 0 OID 16416)
-- Dependencies: 214
-- Data for Name: Osobe; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Osobe" ("OIB", "Ime", "Prezime") FROM stdin;
1	Ime	Prezime
135	Denis	Fajtović
136	Zlatko	Vitez
137	Dubravko	Crnić
138	Josip	Zovko
139	Danijel	Popić
140	Žarko	Ostojić
141	Ljudevit	Aračić
142	Dominik	Bičanić
143	Zvonko	Galjanić
144	Dejan	Baković
145	Vladimir	Bašić
146	Edvard	Vrabec
147	Boris	Belobrajdić
148	Tihomir	Varga
149	Ljudevit	Stojković
150	Ivica	Plazibat
151	Teo	Ljubičić
152	Petar	Ramljak
153	Silvije	Brnabić
154	Dario	Frketić
300	Željka	Bekić
301	Kristina	Zagorac
302	Dorotea	Barić
303	Viktorija	Puškarić
304	Tihana	Bosnić
305	Dubravka	Bartulović
306	Lara	Medved
307	Tamara	Pejaković
308	Leonarda	Kozbašić
309	Monika	Kralj
310	Dubravka	Perić
311	Blaženka	Vidak
312	Lana	Popović
313	Tihana	Kozina
314	Nevenka	Rešetar
315	Vesna	Šimičić
316	Anita	Jakus
317	Lara	Hranilović
318	Danijela	Balić
319	Valentina	Perin
\.


--
-- TOC entry 3175 (class 2606 OID 16420)
-- Name: Osobe osoba_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Osobe"
    ADD CONSTRAINT osoba_pkey PRIMARY KEY ("OIB");


-- Completed on 2022-10-31 12:23:02

--
-- PostgreSQL database dump complete
--

